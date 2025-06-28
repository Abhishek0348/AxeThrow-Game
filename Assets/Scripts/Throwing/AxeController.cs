using UnityEngine;

public class AxeController : MonoBehaviour
{
    public Rigidbody2D rb;
    public float ThrowForce = 10f;

    private bool isAxeThrown = false;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void ThrowAxe(Vector2 force)
    {
        isAxeThrown = true;
        transform.parent = null;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.AddForce(force, ForceMode2D.Impulse);

        animator.SetBool("IsThrown", true);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAxeThrown) return;

        isAxeThrown = false;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

        animator.enabled = false; 

        ContactPoint2D contact = collision.GetContact(0);
        transform.position = contact.point;

        float angle = Mathf.Atan2(-contact.normal.y, -contact.normal.x) * Mathf.Rad2Deg;

        float tiltOffset = Random.Range(-20f, -10f); 
        transform.rotation = Quaternion.Euler(0, 0, angle + tiltOffset);


        if (collision.gameObject.TryGetComponent(out Enemy enemy))
            enemy.OnHit();

        LevelManager.Instance.OnAxeUsed();
    }

}
