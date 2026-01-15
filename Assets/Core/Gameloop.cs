using UnityEngine;

public class Gameloop: MonoBehaviour
{
    const float FRAME_TIME = 1f / 60f;
    float accumulator;


    public InputHandler inputHandler1;
    public MovementHandler movementHandler1;

    public AttackHandler attackHandler1;
    public PlayerComponent player1Component;

    public InputHandler inputHandler2;
    public MovementHandler movementHandler2;
    public AttackHandler attackHandler2;
    public PlayerComponent player2Component;

    public PlayerData playerData1;
    public PlayerData playerData2;
    void Awake()
    {

    }


    void OnEnable()
    {
    }


    void OnDisable()
    {
    }


     
    void Update()
    {
        accumulator += Time.deltaTime;

        while (accumulator >= FRAME_TIME)
        {
            SimulateFrame();

            accumulator -= FRAME_TIME;
        }
    }

    void Start()
    {
        player1Component = new PlayerComponent(playerData1,0);
        player2Component = new PlayerComponent(playerData2,1);
    } 

    void SimulateFrame()
    {
        // process inputs        
        FrameInput input1 = inputHandler1.ConsumeInput();
        FrameInput input2 = inputHandler2.ConsumeInput();

        movementHandler1.Tick(input1, 0);
        attackHandler1.Tick(input1, 0, player1Component, player2Component);

        movementHandler2.Tick(input2, 1);
        attackHandler2.Tick(input2, 1, player2Component, player1Component);

    }
}
