using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DestructibleItem", menuName = "DestructibleItem/Create New DestructibleItem")]
public class DestructableItems : ScriptableObject
{
    public GameObject _destroyedVersion;
}