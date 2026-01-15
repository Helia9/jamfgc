using UnityEditor.UI;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    public LayerMask hurtboxLayer;
    public MoveData lightPunchMoveData;
    public MoveData mediumPunchMoveData;    
    public MoveData heavyPunchMoveData;
    private Rigidbody2D rb;

    public bool isAttacking = false;

    private int moveFrameCount = 0;
    private int moveHitCount = 0;
    public MoveData currentMoveData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    public void Tick(FrameInput input,
                    int pid,
                    PlayerComponent self,
                    PlayerComponent other)
    {
        if (!isAttacking) {
            switch (input) {
                case { lightPunch: true }:
                    StartMove(lightPunchMoveData);
                    break;
                case { mediumPunch: true }:
                    StartMove(mediumPunchMoveData);
                    break;
                case { heavyPunch: true }:
                    StartMove(heavyPunchMoveData);
                    break;
                default:
                    break;
            }
        } else {
            ProcessMove(self, other);
        }

    }

    private void StartMove(MoveData moveData)
    {
        isAttacking = true;
        moveFrameCount = 0;
        currentMoveData = moveData;
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
                other.Damage(currentMoveData.damage);
                moveHitCount++;
            }
            // handle hitboxes activation/states
            // switch on moveframecount -> data->move states
        }
        
    }


}
