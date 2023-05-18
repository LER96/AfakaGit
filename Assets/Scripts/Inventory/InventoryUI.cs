using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    [SerializeField] GameObject emptySlotPrefab;

    private void Start()
    {
        InventoryManager.instance.onInventoryChanged += OnUpdateInventory;
        OnUpdateInventory();
    }

    public void OnUpdateInventory()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        if (InventoryManager.instance.inventory.Count == 0)
            CreateEmptySlot();
        else
        {
            foreach (InventoryItem item in InventoryManager.instance.inventory)
            {
                AddInventorySlot(item);
            }
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(slotPrefab);
        obj.transform.SetParent(transform, false);

        ItemSlot slot = obj.GetComponent<ItemSlot>();
        slot.SetItem(item);
    }

    public void CreateEmptySlot()
    {
        GameObject obj = Instantiate(emptySlotPrefab);
        obj.transform.SetParent(transform, false);
    }
}
