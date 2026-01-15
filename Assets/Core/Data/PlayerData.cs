using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed = 8f;
    public float jumpForce = 14f;
    public int maxHealth = 1000;

    public bool isAttacking = false;
    public int attackFrameCount = 0; // split into 3 [startup|active|recovery]
}
