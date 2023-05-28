using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyWave
{
    public List<Enemy> enemies;
    public float maxdistance;
    public float minimumdistance;
    public bool isLunched;
}
