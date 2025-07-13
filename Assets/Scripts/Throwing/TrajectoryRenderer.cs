using System.Collections.Generic;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    public GameObject dotPrefab;
    public int dotCount = 30;
    public float dotSpacing = 0.1f;

    private List<GameObject> dots = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < dotCount; i++)
        {
            GameObject dot = Instantiate(dotPrefab, transform);
            dot.SetActive(false);
            dots.Add(dot);
        }
    }
    public void ShowTrajectory(Vector3 startPoint, Vector3 endPoint, float force)
    {
        Vector2 direction = (startPoint - endPoint).normalized;
        Vector2 velocity = direction * force;

        Vector3 axePos = LevelManager.Instance.currentAxe.transform.position;

        for (int i = 0; i < dotCount; i++)
        {
            float t = i * dotSpacing;
            Vector2 position = axePos + (Vector3)(velocity * t) + 0.5f * (Vector3)(Physics2D.gravity * t * t);
            dots[i].transform.position = position;
            dots[i].SetActive(true);
        }
    }


    public void HideTrajectory()
    {
        foreach (var dot in dots)
            dot.SetActive(false);
    }
}
