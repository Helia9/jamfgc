using UnityEngine;

[CreateAssetMenu(fileName = "MoveData", menuName = "Scriptable Objects/MoveData")]
public class MoveData : ScriptableObject
{
    public string moveId = "move_001";
    public Vector2 hitboxOffset = Vector2.zero;
    public Vector2 hitboxSize = Vector2.zero; // width & height of the hitbox
    public int damage = 10;
    public int startupFrames = 5;
    public int activeFrames = 3;
    public int recoveryFrames = 10;

    public int maxHits = 1;
    
    public int totalFrames
    {
        get { return startupFrames + activeFrames + recoveryFrames; }
    }

    

    
}
