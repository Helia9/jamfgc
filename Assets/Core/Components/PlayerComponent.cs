using UnityEngine; 


public class PlayerComponent
{

    public PlayerData playerData;
    public int playerIndex;
    public int playerHealth;
    public int ressourceCount;

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


}