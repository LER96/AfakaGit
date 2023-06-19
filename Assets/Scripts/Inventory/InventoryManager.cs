using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Dictionary<Item, InventoryItem> _itemDictionary { get; private set; }
    public List<InventoryItem> inventory = new List<InventoryItem>();
    public Action onInventoryChanged;

    private void Awake()
    {
        instance = this;
        inventory = new List<InventoryItem>();
        _itemDictionary = new Dictionary<Item, InventoryItem>();
    }

    public InventoryItem Get(Item item)
    {
        if (_itemDictionary.TryGetValue(item, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Add(Item item)
    {
        if (_itemDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize >= value.data.maxAmount)
                return;
            else
                value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventory.Add(newItem);
            _itemDictionary.Add(item, newItem);
        }
        
        onInventoryChanged();
    }

    public void Remove(Item item)
    {
        if (_itemDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                _itemDictionary.Remove(item);
            }
        }
    }

    public void OnInventoryChanged()
    {
        onInventoryChanged();
    }
}
