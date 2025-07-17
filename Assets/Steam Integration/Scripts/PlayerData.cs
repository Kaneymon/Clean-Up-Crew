using UnityEngine;

public class PlayerData
{
    public int connectionId;
    public string playerName;
    public CharacterClass classChoice;
    public PlayerController controller;
    public PlayerInventory inventory;

    public PlayerData(int connId, PlayerController controller, string playerName)
    {
        this.connectionId = connId;
        this.controller = controller;
        this.inventory = controller.GetComponent<PlayerInventory>();
        this.playerName = playerName;
        this.classChoice = CharacterClass.Mopper;

    }
}
