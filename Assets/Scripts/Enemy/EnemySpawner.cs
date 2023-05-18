using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemy;
    [SerializeField] float _enemyCount;
    [SerializeField] float _enemySpawnTime = 2f;
    [SerializeField] float _xPos;
    [SerializeField] float _zPos;

    private IEnumerator SpawnEnemy(float time, GameObject enemy)
    {
        while (_enemyCount < 5)
        {
            _xPos = Random.Range(-10, 15);
            _zPos = Random.Range(35, 49);
            yield return new WaitForSeconds(time);
            Instantiate(enemy, new Vector3(_xPos,9, _zPos),Quaternion.identity);
            _enemyCount++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            StartCoroutine(SpawnEnemy(_enemySpawnTime, _enemy));
        }
    }
}
