using UnityEngine;

public class EntityFlipper : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb.linearVelocity.x > 0.1f && !isFacingRight)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < -0.1f && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 newScale = transform.localScale;
        newScale.x *= -1; // Multiplica la escala X por -1
        transform.localScale = newScale;
    }
}