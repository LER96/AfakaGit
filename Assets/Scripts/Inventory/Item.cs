using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class Item : ScriptableObject
{
    public int maxAmount;
    public string itemName;
    public int value;
    public Sprite icon;
    public GameObject prefab;
    public float dropChance;
}
