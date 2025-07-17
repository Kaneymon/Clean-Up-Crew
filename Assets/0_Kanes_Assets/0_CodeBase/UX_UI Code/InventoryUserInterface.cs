using UnityEngine;
using UnityEngine.UI;

public class InventoryUserInterface : MonoBehaviour
{
    [SerializeField] InventorySlotPrefab[] slots;
    [SerializeField] PlayerInventory inventory;

    public void UpdateInventorySlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            string id = inventory.inventory[i].itemId;
            int quantity = inventory.inventory[i].stackCount;
            slots[i].UpdateSlot(id, quantity);
        }
    }



    private void FixedUpdate()
    {

        UpdateInventorySlots();

    }
}
