using FishNet.Object;
using UnityEngine;

public class TestObject : NetworkBehaviour, IInteractable
{
    [SerializeField] private ItemData data;

    public void Interaction(PlayerController player)
    {
        // Client initiates interaction with their ID
        ServerInteraction(player.OwnerId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerInteraction(int playerId)
    {
        Debug.Log($"[Server] {nameof(TestObject)} interacted with by player {playerId}");

        var playerData = PlayerManager.Instance.GetPlayer(playerId);

        Debug.Log("player data found");

        if (playerData == null)
        {
            Debug.LogWarning($"[Server] No player data found for connection ID {playerId}");
            return;
        }

        if (playerData.inventory != null)
        {
            Debug.Log("player inventory found");
            playerData.inventory.AddItemServer(data.itemID, 2);
        }
        else
        {
            Debug.LogWarning($"[Server] Player {playerId} has no inventory component.");
        }

        Despawn(); // Always despawn regardless of inventory state
    }
}
