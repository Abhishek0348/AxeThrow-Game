using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void OnHit()
    {
        LevelManager.Instance.OnEnemyHit();
    }
}
