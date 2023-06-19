using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    void Pickup()
    {
        if (InventoryManager.instance._itemDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.stackSize >= value.data.maxAmount)
                return;
            else
                Destroy(this.gameObject);
        }

        InventoryManager.instance.Add(item);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Pickup();
        }
    }
}