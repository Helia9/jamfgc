using Unity.VisualScripting;
using UnityEngine;

public class MovementHandler: MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 14f;

    private Rigidbody2D rb;
    public bool isGrounded { get; private set;}


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Tick(FrameInput input, int pid)
    {
        rb.linearVelocity = new Vector2(input.moveX * moveSpeed, rb.linearVelocity.y);
        if (input.jump && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
