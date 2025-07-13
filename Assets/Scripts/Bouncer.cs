using UnityEngine;

public class Bouncer : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null)
        {
            Rigidbody2D rb = collision.rigidbody;

            Vector2 incomingVelocity = rb.linearVelocity;
            Vector2 normal = collision.contacts[0].normal;
            Vector2 bounce = Vector2.Reflect(incomingVelocity, normal);

            rb.linearVelocity = bounce;
        }
    }
}
