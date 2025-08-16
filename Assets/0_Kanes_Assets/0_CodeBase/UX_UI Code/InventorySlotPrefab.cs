using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotPrefab : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] Sprite DefaultImage;
    [SerializeField] TMP_Text slotCountText;
    [SerializeField] ItemDatabase allItems;
    public void UpdateSlot(string itemID, int quantity)
    {
        UpdateSlotImage(itemID);
        UpdateSlotText(quantity);      
    }

    private void UpdateSlotImage(string itemId)
    {
        if(itemId == "Empty") 
        { 
            itemImage.sprite = DefaultImage; 
            return; 
        }
        else
        {
            itemImage.sprite = allItems.GetItemByID(itemId).icon;
        }
    }

    private void UpdateSlotText(int quantity)
    {
        if (quantity > 0)
        {
            slotCountText.text = quantity.ToString();
        }
        else
        {
            slotCountText.text = string.Empty;
        }
    }
}
