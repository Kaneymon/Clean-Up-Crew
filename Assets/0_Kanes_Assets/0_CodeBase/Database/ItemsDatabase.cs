using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    private Dictionary<string, ItemData> idLookup;

    public void Initialize() //create the items dictionary
    {
        idLookup = new Dictionary<string, ItemData>();
        foreach (var item in items)
        {
            idLookup[item.itemID] = item;
        }
    }

    public ItemData GetItemByID(string id)
    {
        if (idLookup == null)
            Initialize();

        return idLookup.TryGetValue(id, out var item) ? item : null;
    }
}

