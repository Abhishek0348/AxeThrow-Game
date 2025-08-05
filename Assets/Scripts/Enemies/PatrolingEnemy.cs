using UnityEngine;

public class PatrollingEnemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f; 

    private Rigidbody2D rb;
    private Transform currentPoint;
    private SpriteRenderer sr;

    private bool isAlive = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentPoint = pointA;
    }

    private void Update()
    {
        if (isAlive)
            MoveToTarget();
    }

    private void MoveToTarget()
    {
        float direction = currentPoint.position.x - transform.position.x;
        rb.linearVelocity = new Vector2(Mathf.Sign(direction) * speed, rb.linearVelocity.y);

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            Flip();
            currentPoint = currentPoint == pointA ? pointB : pointA;
        }
    }

    public void StopPatrolling() 
    {
        isAlive = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static; 
    }

    private void Flip()
    {
        sr.flipX = !sr.flipX;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null)
            Gizmos.DrawWireSphere(pointA.position, 0.3f);
        if (pointB != null)
            Gizmos.DrawWireSphere(pointB.position, 0.3f);
    }
}
