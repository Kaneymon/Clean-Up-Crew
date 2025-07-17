using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemID;

    [TextArea]
    public string description;

    // Example stats (expand as needed)
    public float weight;
    public bool isStackable;

    //reference type specific data would be good to have here too. dmg, reload time, rarity, radioactivity, etc.
}
