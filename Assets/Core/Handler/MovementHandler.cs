using System.Runtime.Serialization.Formatters;
using Unity.VisualScripting;
using UnityEngine;

public class MovementHandler: MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpForce = 14f;

    private Rigidbody2D rb;
    private Collider2D col;
    public bool isGrounded { get; private set;}


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        // rb.transform.position = new Vector3(0, -1, 0);
    }

    public void Tick(FrameInput input, int pid, PlayerComponent self)
    {

        self.TickKnockback(rb);
        if (self.ProcessHitlag()) {
            return;
        }


        if (!(input.moveY < -0.5f)) {
        rb.linearVelocity = new Vector2(input.moveX * moveSpeed, rb.linearVelocity.y);
        self.CoordX = rb.transform.position.x;
        }
        if (input.jump && isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
        if (input.moveX > 0.5f && self.facing == PlayerComponent.Direction.Left) {
            // back
            self.isBlocking = true;
        } else if (input.moveX < -0.5f && self.facing == PlayerComponent.Direction.Right) {
            // other back
            self.isBlocking = true;
        }
        else {
            self.isBlocking = false;
        }
        if (input.moveY < -0.5f) {
            self.isCrouching = true;
            //col.offset = new Vector2(col.offset.x, -0.0f);
            //col.transform.localScale = new Vector3(1f, 0.5f, 1f);
        } else {
            self.isCrouching = false;
            //col.offset = new Vector2(col.offset.x, 0f);
            //col.transform.localScale = new Vector3(1f, 1f, 1f);
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
