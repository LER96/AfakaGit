using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{ 
    public Item item;
  
    void Pickup()
    {
        InventoryManager.instance.Add(item);
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Pickup();

        }
    }
}
