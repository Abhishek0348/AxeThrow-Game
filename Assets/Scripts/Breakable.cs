using UnityEngine;

public class Breakable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
            // Destroy the axe
            Destroy(collision.gameObject);
            Destroy(gameObject);
    }
}
