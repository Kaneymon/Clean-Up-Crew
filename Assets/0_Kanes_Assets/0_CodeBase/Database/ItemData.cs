using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    [TextArea]
    public string description;
    public Sprite icon;
    public bool isStackable;
    public int maxStackSize = 1;

    public GameObject prefab; // The world prefab representing this item

#if UNITY_EDITOR
    private void OnValidate() //check if item ID's are duplicated
    {
        string path = AssetDatabase.GetAssetPath(this);
        var allItems = AssetDatabase.FindAssets("t:ItemData");

        foreach (var guid in allItems)
        {
            string otherPath = AssetDatabase.GUIDToAssetPath(guid);
            if (otherPath == path) continue; // skip self

            ItemData other = AssetDatabase.LoadAssetAtPath<ItemData>(otherPath);
            if (other != null && other.itemID == itemID && !string.IsNullOrEmpty(itemID))
            {
                Debug.LogError($"Duplicate itemID found: '{itemID}' in {path} and {otherPath}");
            }
        }
    }
#endif
}
