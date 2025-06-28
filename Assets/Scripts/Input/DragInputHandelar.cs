using UnityEngine;

public class DragInputHandler : MonoBehaviour
{
    public TrajectoryRenderer trajectoryRenderer;
    public float maxDragDistance = 2f;
    public float maxForce = 15f;
    private bool isDragging = false;

    private Vector3 dragStartPoint;
    private Vector3 currentDragPoint;

    public Camera Camera;

    private void Start()
    {
        if (Camera == null)
            Camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDragging)
            StartDrag();

        if (isDragging)
            Drag();

        if (Input.GetMouseButtonUp(0) && isDragging)
            EndDrag();
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 pos = Camera.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }

    void StartDrag()
    {
        isDragging = true;
        dragStartPoint = GetMouseWorldPosition();
    }

    void Drag()
    {
        currentDragPoint = GetMouseWorldPosition();

        Vector3 dragVector = currentDragPoint - dragStartPoint;
        if (dragVector.magnitude > maxDragDistance)
            dragVector = dragVector.normalized * maxDragDistance;

        Vector3 clampedEndPoint = dragStartPoint + dragVector;
        float FinalForce = dragVector.magnitude / maxDragDistance * maxForce;

        trajectoryRenderer.ShowTrajectory(dragStartPoint, clampedEndPoint, FinalForce);
    }

    void EndDrag()
    {
        isDragging = false;

        Vector3 dragVector = currentDragPoint - dragStartPoint;
        if (dragVector.magnitude > maxDragDistance)
            dragVector = dragVector.normalized * maxDragDistance;

        Vector3 force = (dragStartPoint - (dragStartPoint + dragVector)).normalized * (dragVector.magnitude / maxDragDistance * maxForce);

        GameObject axeObj = LevelManager.Instance.currentAxe;
        if (axeObj != null)
        {
            AxeController axe = axeObj.GetComponent<AxeController>();
            if (axe != null)
            {
                axe.ThrowAxe(force);
            }
        }

        trajectoryRenderer.HideTrajectory();
    }

}
