using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] float _countdown;
    [SerializeField] public Wave[] _waves;
    [SerializeField] public int _waveIndex = 0;

    [SerializeField] GameObject _spawnPoint;

    bool _readyToSpawn;
    private void Update()
    {
        _countdown -= Time.deltaTime;
        if (_countdown <= 0)
        {
            _readyToSpawn = false;
            _countdown = _waves[_waveIndex]._durationToNextWave;
            StartCoroutine(SpawnWave());
        }

        if (_waves[_waveIndex]._enemiesLeft == 0)
        {
            _readyToSpawn = true;
            _waveIndex++;
        }
    }

    private void Start()
    {
        _readyToSpawn = true;
        for (int i = 0; i < _waves.Length; i++)
        {
            _waves[i]._enemiesLeft = _waves[i]._enemies.Length;
        }
    }

    public IEnumerator SpawnWave() 
    {
        for (int i = 0; i < _waves[_waveIndex]._enemies.Length; i++)
        {
            Enemy enemy = Instantiate(_waves[_waveIndex]._enemies[i], _spawnPoint.transform);
            enemy.transform.SetParent(_spawnPoint.transform);
            yield return new WaitForSeconds(_waves[_waveIndex]._durationBeforeNextEnemy);
        }
    }
}

[System.Serializable]
public class Wave
{
    public Enemy[] _enemies;
    public float _durationBeforeNextEnemy;
    public float _durationToNextWave = 5;

    public int _enemiesLeft;
}
