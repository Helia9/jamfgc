using UnityEditor.UI;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public LayerMask hurtboxLayer;
    public MoveData lightPunchMoveData;
    public MoveData mediumPunchMoveData;    
    public MoveData heavyPunchMoveData;
    private Rigidbody2D rb;
    public Animator animator;

    public bool isAttacking = false;

    private int moveFrameCount = 0;
    private int moveHitCount = 0;
    public MoveData currentMoveData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    public void Tick(FrameInput input,
                    int pid,
                    PlayerComponent self,
                    PlayerComponent other)
    {
        if (!isAttacking) {
            switch (input) {
                case { lightPunch: true }:
                    StartMove(lightPunchMoveData, "LightPunch");
                    break;
                case { mediumPunch: true }:
                    StartMove(mediumPunchMoveData, "MediumPunch");
                    break;
                case { heavyPunch: true }:
                    StartMove(heavyPunchMoveData, "HeavyPunch");
                    break;
                default:
                    break;
            }
        } else {
            ProcessMove(self, other);
        }

    }

    private void StartMove(MoveData moveData, string animationTrigger)
    {
        isAttacking = true;
        moveFrameCount = 0;
        currentMoveData = moveData;
        animator.SetTrigger(animationTrigger);
    }

    private void ProcessMove(PlayerComponent self, PlayerComponent other)
    {
        moveFrameCount++;
        if (moveFrameCount > currentMoveData.totalFrames){
            isAttacking = false;
            moveFrameCount = 0;
            moveHitCount = 0;
            return;
        } else {
            if (moveHitCount >= currentMoveData.maxHits) {
                return;
            }
            Collider2D[] hits = Physics2D.OverlapBoxAll(
                rb.position + currentMoveData.hitboxOffset,
                currentMoveData.hitboxSize,
                0f,
                hurtboxLayer
            );
            foreach (Collider2D hit in hits)
            {
                Debug.Log("Hit: " + hit.name);
                if (other.isBlocking) {
                    Debug.Log("Blocked!");
                    moveHitCount++;
                    continue;
                }
                other.Damage(currentMoveData.damage);
                moveHitCount++;
                other.ApplyHitlag(currentMoveData.hitlagFrames);
                other.ApplyKnockback(
                    currentMoveData.knockbackDirection,
                    currentMoveData.knockbackForce,
                    currentMoveData.knockbackFrames
                );

            }
            // handle hitboxes activation/states
            // switch on moveframecount -> data->move states
        }
        
    }


}
