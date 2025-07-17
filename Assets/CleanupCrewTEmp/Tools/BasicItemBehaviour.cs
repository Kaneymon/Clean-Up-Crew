using UnityEngine;


public class BasicItemBehaviour : MonoBehaviour, IActionable, IInteractable
{
    [SerializeField] private Item itemData;

    public void ActionOne() {}
    public void ActionTwo() {}

    public void Interaction(PlayerController player)
    {
       // if (!player.IsOwner) return; // Only owning client requests pickup

        RequestPickupServer(player.GetInventory(), itemData);
    }

    private void RequestPickupServer(PlayerInventory inventory, Item item)
    {
        // Validate inventory add logic on server side here if needed
        /*
bool added = inventory.AddItemServer(item);

if (added)
{
    // Destroy the item on server, which syncs destruction to all clients
    // OR if no NetworkObject component:
    // Destroy(gameObject);
}
else
{
    // Optionally notify owner client of failure (inventory full)
}*/
    }
}

/* Make sure your pickup prefab has a NetworkObject component attached for NetworkObject.Despawn() to work.
Summary:

    Client owning the player requests pickup via ServerRpc.

    Server adds item, validates, then despawns the pickup GameObject for all clients.

    This prevents cheating and keeps all clients in sync.

*/
