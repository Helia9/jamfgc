using UnityEngine;

public class Actions: MonoBehaviour
{

  public Vector2 hitboxOffset = new Vector2(1f, 0f);   // where the hitbox is relative to the fighter
    public Vector2 hitboxSize = new Vector2(1.2f, 0.6f); // width & height of the hitbox
    public LayerMask hurtboxLayer; // layer that opponents' hurtboxes are on


    public void Attack()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 origin = rb.position;

        // If your character can face left/right, flip the offset
        float facing = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 hitboxCenter = origin + new Vector2(hitboxOffset.x * facing, hitboxOffset.y);

        Debug.Log("Hitbox center: " + hitboxCenter);

        // Find everything inside the hitbox
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            hitboxCenter,
            hitboxSize,
            0f,
            hurtboxLayer
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: " + hit.name);

            // Here is where you would apply damage, hitstun, etc.
            // hit.GetComponent<Fighter>().TakeHit(...);
        }
    
    void OnDrawGizmosSelected()
{
    if (!Application.isPlaying) return;

    Rigidbody2D rb = GetComponent<Rigidbody2D>();
    if (rb == null) return;

    float facing = transform.localScale.x > 0 ? 1f : -1f;
    Vector2 center = rb.position + new Vector2(hitboxOffset.x * facing, hitboxOffset.y);

    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(center, hitboxSize);
}
}
}
