using UnityEngine;

public class InputBuffer
{
    private int moveX;

    private int moveY;
    private bool jump;
    private bool mediumPunch;
    private bool heavyPunch;    
    private bool lightPunch;

    public void SetMoveX(int value)
    {
        moveX = value;
    }

    public void SetMoveY(int value)
    {
        moveY = value;
    }

    public void SetJump()
    {
        jump = true;
    }

    public void SetMediumPunch()
    {
        mediumPunch = true;
    }

    public void SetHeavyPunch()
    {
        heavyPunch = true;
    }

    public void SetLightPunch()
    {
        lightPunch = true;
    }


    private void clearInputs()
    {
        jump = false;
        mediumPunch = false;
        heavyPunch = false;
        lightPunch = false;
    }

    public FrameInput Consume()
    {
        var input = new FrameInput
        {
            moveX = moveX,
            moveY = moveY,
            jump = jump,
            mediumPunch = mediumPunch,
            heavyPunch = heavyPunch,
            lightPunch = lightPunch
        };
        clearInputs();
        return input;
    }
}