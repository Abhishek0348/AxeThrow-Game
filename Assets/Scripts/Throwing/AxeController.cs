using UnityEngine;

public class AxeController : MonoBehaviour
{
    public Rigidbody2D rb;
    private bool isAxeThrown = false;
    private bool hasBounced = false; // 🆕

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ThrowAxe(Vector2 force)
    {
        isAxeThrown = true;
        hasBounced = false; // reset bounce state 🆕
        transform.parent = null;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.AddForce(force, ForceMode2D.Impulse);
        animator.SetBool("IsThrown", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Let bouncer handle the bounce
        if (collision.gameObject.CompareTag("Bouncer") && !hasBounced)
        {
            hasBounced = true;
            return; // skip default behavior
        }

        if (!isAxeThrown) return;
        isAxeThrown = false;

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            HandleStick(collision);
            enemy.OnHit();
        }
        else
        {
            HandelDestroy(collision);
        }

        LevelManager.Instance.OnAxeUsed();
    }


    private void HandelDestroy(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void HandleStick(Collision2D collision)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator.enabled = false;

        ContactPoint2D contact = collision.GetContact(0);
        transform.position = contact.point;

        float angle = Mathf.Atan2(-contact.normal.y, -contact.normal.x) * Mathf.Rad2Deg;
        float tiltOffset = Random.Range(-20f, -10f);
        transform.rotation = Quaternion.Euler(0, 0, angle + tiltOffset);
    }
}
