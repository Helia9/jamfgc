using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{
    const float FRAME_TIME = 1f / 60f;
    float accumulator;
    public float moveSpeed = 8f;
    public float jumpForce = 14f;


    public struct MoveData
    {
        public Vector2 hitboxOffset ;
        public Vector2 hitboxSize;
    }
    Rigidbody2D rb;
    PlayerControls controls;

    Vector2 moveInput;
    bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new PlayerControls();
    }

    public struct FrameInput
{
    public int moveX;     // -1, 0, 1
    public bool jump;
    public bool mediumPunch;
    public bool heavyPunch;
    public bool lightPunch;
}

FrameInput pendingInput;


void OnEnable()
{
    controls.Player.Enable();

    controls.Player.Move.performed += ctx =>
    {
        Vector2 v = ctx.ReadValue<Vector2>();
        pendingInput.moveX = Mathf.RoundToInt(v.x);
    };

    controls.Player.Move.canceled += _ =>
    {
        pendingInput.moveX = 0;
    };

    controls.Player.Jump.performed += _ =>
    {
        pendingInput.jump = true;
    };

    controls.Player.MediumPunch.performed += _ =>
    {
        pendingInput.mediumPunch = true;
    };

    controls.Player.HeavyPunch.performed += _ =>
    {
        pendingInput.heavyPunch = true;
    };
    controls.Player.LightPunch.performed += _ =>
    {
        pendingInput.lightPunch = true;
    };
}

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
            accumulator += Time.unscaledDeltaTime;

        while (accumulator >= FRAME_TIME)
        {
            SimulateFrame();
            accumulator -= FRAME_TIME;
        }
    }


    void clearInputs()
    {
        pendingInput.jump = false;
        pendingInput.mediumPunch = false;
        pendingInput.heavyPunch = false;
        pendingInput.lightPunch = false;
        //pendingInput.moveX = 0;
    }
    void SimulateFrame()
    {
        FrameInput input = pendingInput;
        clearInputs();
         rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        if (isAttacking)
        {
            attackFrameCount--;
            if (attackFrameCount <= 0)
            {
                isAttacking = false;
                attackFrameCount = 0;
            }
        }
        ProcessMovements(input);
        ProcessAttacks(input);  
    }

    void ProcessMovements(FrameInput input)
    {
        moveInput = new Vector2(input.moveX, 0);
        if (input.jump)
        {
            TryJump();
        }
    }

    void ProcessAttacks(FrameInput input)
    {
        if (input.mediumPunch)
        {
            MediumPunch();
        }
        if (input.heavyPunch)
        {
            HeavyPunch();
        }
        if (input.lightPunch)
        {
            LightPunch();
        }
    }

    void FixedUpdate()
    {
       
        
    }

    void TryJump()
    {
        if (!isGrounded)
        {
            return;
        }
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("grounded");
        }
        else
        {
            Debug.Log("exit without degrounding");
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    //public Vector2 hitboxOffset = new Vector2(1f, 3f);   // where the hitbox is relative to the fighter
    //public Vector2 hitboxSize = new Vector2(1.2f, 4.6f); // width & height of the hitbox
    public LayerMask hurtboxLayer; 

    private MoveData currentMove = new MoveData();
    public int hitboxDuration = 20; // idk
    public bool isAttacking = false;
    public int attackFrameCount = 0;

    public void HeavyPunch()
    {
        int attackDuration = 40;
        if (isAttacking)
        {
            Debug.Log("Already attacking, cannot initiate another attack.");
            return;
        }

        currentMove.hitboxOffset = new Vector2(2.5f, 0.5f);
        currentMove.hitboxSize = new Vector2(2.0f, 10.0f);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 origin = rb.position;
        float facing = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 hitboxCenter = origin + new Vector2(currentMove.hitboxOffset.x * facing, currentMove.hitboxOffset.y);
        isAttacking = true;
        hitboxDuration = attackDuration;
        attackFrameCount = attackDuration;
                Collider2D[] hits = Physics2D.OverlapBoxAll(
            hitboxCenter,
            currentMove.hitboxSize,
            0f,
            hurtboxLayer
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: " + hit.name);


        }
    }


    public void MediumPunch()
    {
        if (isAttacking)
        {
            Debug.Log("Already attacking, cannot initiate another attack.");
            return;
        }
        currentMove.hitboxOffset = new Vector2(1.5f, 0.5f);
        currentMove.hitboxSize = new Vector2(1.5f, 3.0f);
        int attackDuration = 20;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 origin = rb.position;

        float facing = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 hitboxCenter = origin + new Vector2(currentMove.hitboxOffset.x * facing, currentMove.hitboxOffset.y);

        isAttacking = true;
        hitboxDuration = attackDuration;
        attackFrameCount = attackDuration;
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            hitboxCenter,
            currentMove.hitboxSize,
            0f,
            hurtboxLayer
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: " + hit.name);


        }



    }

    public void LightPunch()
    {
 if (isAttacking)
        {
            Debug.Log("Already attacking, cannot initiate another attack.");
            return;
        }
        currentMove.hitboxOffset = new Vector2(1.5f, 0.5f);
        currentMove.hitboxSize = new Vector2(6.5f, 1.0f);
        int attackDuration = 6;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        Vector2 origin = rb.position;

        float facing = transform.localScale.x > 0 ? 1f : -1f;
        Vector2 hitboxCenter = origin + new Vector2(currentMove.hitboxOffset.x * facing, currentMove.hitboxOffset.y);

        isAttacking = true;
        hitboxDuration = attackDuration;
        attackFrameCount = attackDuration;
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            hitboxCenter,
            currentMove.hitboxSize,
            0f,
            hurtboxLayer
        );

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: " + hit.name);


        }



    }

















            void OnDrawGizmosSelected()
{
            if (!Application.isPlaying) return;

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb == null) return;
            if (!isAttacking) return;
            float facing = transform.localScale.x > 0 ? 1f : -1f;
            Vector2 center = rb.position + new Vector2(currentMove.hitboxOffset.x * facing, currentMove.hitboxOffset.y);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(center, currentMove.hitboxSize);
        }
}

