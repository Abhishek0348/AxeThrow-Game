using UnityEngine;

public class DragInputHandler : MonoBehaviour
{
    public TrajectoryRenderer trajectoryRenderer;
    public float maxForce = 30f;        
    public float maxDragDistance = 1f;
    private bool isDragging = false;

    private Vector3 dragStartPoint;
    private Vector3 currentDragPoint;

    private Camera Camera;

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
        LevelManager.Instance.playerAnimator.SetTrigger("StartThrow");
    }

    void Drag()
    {
        currentDragPoint = GetMouseWorldPosition();

        Vector3 dragVector = currentDragPoint - dragStartPoint;
        dragVector = Vector3.ClampMagnitude(dragVector, maxDragDistance);

        float dragFactor = dragVector.magnitude / maxDragDistance;
        float force = dragFactor * maxForce;

        Vector3 clampedEndPoint = dragStartPoint + dragVector;
        trajectoryRenderer.ShowTrajectory(dragStartPoint, clampedEndPoint, force);
    }

    void EndDrag()
    {
        isDragging = false;

        Vector3 dragVector = currentDragPoint - dragStartPoint;
        dragVector = Vector3.ClampMagnitude(dragVector, maxDragDistance);

        float dragFactor = dragVector.magnitude / maxDragDistance;
        float throwForce = dragFactor * maxForce; // Linear force scale

        Vector3 throwDirection = -dragVector.normalized;
        Vector3 force = throwDirection * throwForce;

        if (LevelManager.Instance.currentAxe.TryGetComponent(out AxeController axe))
        {
            axe.ThrowAxe(force);
            LevelManager.Instance.playerAnimator.SetTrigger("FinishThrow");
        }

        trajectoryRenderer.HideTrajectory();
    }
}
