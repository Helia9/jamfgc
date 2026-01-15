using UnityEngine;


public class InputHandler : MonoBehaviour
{
    public int playerId = 1;
    private PlayerControls controls;
    private InputBuffer inputBuffer = new InputBuffer();

    private void Awake()
    {
        controls = new PlayerControls();
    } 

    private void OnEnable()
    {
        controls.Player.Enable();

        if (playerId == 1) {
        controls.Player.Move.performed += ctx =>
        {
            Vector2 v = ctx.ReadValue<Vector2>();
            // deadzone for controller
            if (v.x > 0.5f || v.x < -0.5f) {
            inputBuffer.SetMoveX(Mathf.RoundToInt(v.x));
            }
        }; }
        else
        {
             controls.Player.Move1.performed += ctx =>
        {
            Vector2 v = ctx.ReadValue<Vector2>();
            if (v.x > 0.5f || v.x < -0.5f) {
            inputBuffer.SetMoveX(Mathf.RoundToInt(v.x));
            }
        };
        }

        controls.Player.Move.canceled += _ =>
        {
            inputBuffer.SetMoveX(0);
        };


        controls.Player.Move1.canceled += _ =>
        {
            inputBuffer.SetMoveX(0);
        };

        if (playerId == 1) {
        controls.Player.Move.performed +=  ctx =>
        {
            // deadzone for controller 
            if (ctx.ReadValue<Vector2>().y > 0.5) {
                inputBuffer.SetJump();
            }
        };
        } else {
         controls.Player.Move1.performed +=  ctx =>
        {
            if (ctx.ReadValue<Vector2>().y > 0.5) {
                inputBuffer.SetJump();
            }
        };   
        }

        controls.Player.MediumPunch.performed += _ =>
        {
            inputBuffer.SetMediumPunch();
        };

        controls.Player.HeavyPunch.performed += _ =>
        {
            inputBuffer.SetHeavyPunch();
        };

        controls.Player.LightPunch.performed += _ =>
        {
            inputBuffer.SetLightPunch();
        };
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    public FrameInput ConsumeInput()
    {
        return inputBuffer.Consume();
    }

}