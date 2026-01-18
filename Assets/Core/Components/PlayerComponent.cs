using UnityEngine; 


public class PlayerComponent
{

    public enum Direction { Left, Right }
    public PlayerData playerData;
    public int playerIndex;
    public int playerHealth;
    public int ressourceCount;

    public float CoordX = 0.0f;

    private int hitLagFrames = 0;

    private int IFrames = 0;

    private Vector2 kbDirection = Vector2.zero;
    private float kbForce = 0f;
    private int kbFrames = 0;
    private int kbDecay = 1;

    public Direction facing = Direction.Right;

    public bool isCrouching = false;
    
    public bool isBlocking = false;
    
    public int moveStartup = 0;
    public int moveActive = 0;
    public int moveRecovery = 0;
    public int moveTotalFrames = 0;

    public int moveMaxFrames = 0;
    public Direction moveDirection = Direction.Right;
    public Vector2 startupMoveDisplacement = Vector2.zero;
    public float startupMoveDisplacementSpeed = 1f;
    public Vector2 activeMoveDisplacement = Vector2.zero;
    public float activeMoveDisplacementSpeed = 1f;
    public Vector2 recoveryMoveDisplacement = Vector2.zero;
    public float recoveryMoveDisplacementSpeed = 1f;


    public void applyMoveDisplacement(Rigidbody2D rb)
    {
        int currentFrame = moveMaxFrames - moveTotalFrames;
        if (currentFrame <= moveStartup)
        {
            rb.MovePosition(rb.position + new Vector2(
                startupMoveDisplacement.x * (moveDirection == Direction.Right ? 1 : -1) * startupMoveDisplacementSpeed,
                startupMoveDisplacement.y * startupMoveDisplacementSpeed));
        }
        if (currentFrame > moveStartup && currentFrame <= moveStartup + moveActive)
        {
            rb.MovePosition(rb.position + new Vector2(
                activeMoveDisplacement.x * (moveDirection == Direction.Right ? 1 : -1) * activeMoveDisplacementSpeed,
                activeMoveDisplacement.y * activeMoveDisplacementSpeed));
        }
        if (currentFrame > moveStartup + moveActive)
        {
            rb.MovePosition(rb.position + new Vector2(
                recoveryMoveDisplacement.x * (moveDirection == Direction.Right ? 1 : -1) * recoveryMoveDisplacementSpeed,
                recoveryMoveDisplacement.y * recoveryMoveDisplacementSpeed));
        }
    }
    
    public bool processMoveFrames(Rigidbody2D rb)
    {
        if (moveTotalFrames > 0)
        {
            moveTotalFrames--;
            applyMoveDisplacement(rb);
            return true;
        }
        return false;
    }

    public PlayerComponent(PlayerData data, int index )
    {
        playerData = data;
        playerIndex = index;
        playerHealth = playerData.maxHealth;
        ressourceCount = 0;
    }


    public void Damage(int damage)
    {
        Debug.Log("player " + playerIndex + " took " + damage + " damage. They're now at " + (playerHealth - damage) + " health.");
        playerHealth -= damage;
        if (playerHealth < 0) playerHealth = 0;
    }

    public bool ProcessHitlag()
    {
        if (hitLagFrames > 0) {
            hitLagFrames--;
            return true;
        }
        return false;
    }
    public void ApplyHitlag(int frames)
    {
        hitLagFrames = frames;
    }

    public void ApplyMoveData(MoveData move)
    {
        moveStartup = move.startupFrames;
        moveActive = move.activeFrames;
        moveRecovery = move.recoveryFrames;
        moveTotalFrames = move.totalFrames;
        moveMaxFrames = move.totalFrames;
        startupMoveDisplacement = move.startupDisplacement;
        activeMoveDisplacement = move.activeDisplacement;
        recoveryMoveDisplacement = move.recoveryDisplacement;
        moveDirection = facing;
        startupMoveDisplacementSpeed = move.startupDisplacementSpeed;
        activeMoveDisplacementSpeed = move.activeDisplacementSpeed;
        recoveryMoveDisplacementSpeed = move.recoveryDisplacementSpeed;
    }
    public void ApplyKnockback(Vector2 direction, float force, int frames, int multiplier)
    {
        //kbDirection = direction.normalized * multiplier;
        kbDirection.x = direction.normalized.x * multiplier;
        kbDirection.y = direction.normalized.y;
        kbForce = force;
        kbFrames = frames;
    }

    public void TickKnockback(Rigidbody2D rb)
    {
        if (kbFrames > 0)
        {
            rb.AddForce(kbDirection * kbForce, ForceMode2D.Impulse);
            kbFrames--;
            kbForce = Mathf.Max(0, kbForce - kbDecay);
        }
    }


    // wrapper tbh
    public void Hit(MoveData move)
    {
        if (IFrames > 0) {
            // process I frames hit
        } else {
            Damage(move.damage);
        }
        //ApplyHitlag(move.hitlagFrames);
        //ApplyKnockback(move.knockbackDirection, move.knockbackForce, move.knockbackFrames);
    }

}