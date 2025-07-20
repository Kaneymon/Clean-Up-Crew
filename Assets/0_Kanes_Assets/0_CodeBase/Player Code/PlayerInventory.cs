using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class PlayerInventory : NetworkBehaviour
{
    //SyncLists automatically synchronize changes to clients

    public readonly SyncList<InventorySlot> inventory = new SyncList<InventorySlot>();
    ItemDatabase allItems;

    //reference a document of all items?
    [ServerRpc]
    public void AddItemServer(string newItemsId, int quantity = 1)
    {
        if (inventory.Count <= 0) { Debug.Log("inventory synclist is not initialised yet bro"); }
        int inventoryIndex = GetNextFreeInventorySlot();
        if (inventoryIndex == -1)
        {
            return;
        }
        // Try stack first
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemId == newItemsId
                && allItems.GetItemByID(newItemsId).isStackable
                && inventory[i].stackCount + quantity <= allItems.GetItemByID(newItemsId).maxStackSize)//if exists in inventory, is stackable, and not at max stack size.
            {
                var slot = inventory[i];
                slot.stackCount += quantity;
                inventory[i] = slot; // SyncList requires re-assign to trigger sync
                return;
            }
        }

        // Add new slot if space available
        inventory.Insert(inventoryIndex, new InventorySlot(newItemsId, quantity));
    }

    [ServerRpc]
    public void RemoveItemServer(int index, int quantity = 1)
    {
        if (index < 0 || index >= inventory.Count) return;

        InventorySlot slot = inventory[index];
        slot.stackCount -= quantity;

        if (slot.stackCount <= 0)
            inventory[index] = new InventorySlot("Empty", 0);
        else
            inventory[index] = slot;
    }

    [ServerRpc]
    public void DropItemServer(int index, int quantity = 1)
    {
        //needs to spawn an item from the inventory.
    }
     
    private void InitializeInventory()
    {
        for(int i = 0; i < 6; i++)
        {
            inventory.Add(new InventorySlot("Empty", 0));
        }
    }

    public int GetNextFreeInventorySlot()
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemId == "Empty")
            {
                return i;
            }
        }
        Debug.Log("no free InventorySlots");
        return -1; // fail outcome.
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        if (IsServerInitialized)
            InitializeInventory();
    }

}

public struct InventorySlot
{
    public string itemId;
    public int stackCount;

    public InventorySlot(string ItemId, int StackCount)
    {
        this.itemId = ItemId;
        this.stackCount = StackCount;
    }
}

