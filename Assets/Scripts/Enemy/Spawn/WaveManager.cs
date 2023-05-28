using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [SerializeField] float delayBetweenWave;
    [SerializeField] List<Enemy> _enemiesCopy;
    [SerializeField] List<EnemyWave> waves;

    float _currentSpawned;
    EnemyWave _currentWave;
    Transform _player;
    Vector3 _dirrection;

    private void Start()
    {
        _currentWave = waves[0];
    }

    private void Update()
    {
        if (_currentWave != null)
        {
            for (int i = 0; i < _enemiesCopy.Count; i++)
            {
                if (_enemiesCopy[i].IsDead)
                {
                    _enemiesCopy.Remove(_enemiesCopy[i]);
                }

                if (_enemiesCopy.Count == 0)
                {
                    ChangeWave();
                    break;
                }
            }
        }
    }

    void ChangeWave()
    {
        waves.Remove(_currentWave);
        if (waves.Count > 0)
        {
            _currentWave = waves[0];
            _currentSpawned = 0;
        }
        else
            _currentWave = null;

        if (_currentWave != null)
        {
            StopAllCoroutines();
            StartCoroutine(ExtraTimeBetweenWaves());
        }

    }

    IEnumerator ExtraTimeBetweenWaves()
    {
        yield return new WaitForSeconds(delayBetweenWave);
        GenerateWave();
    }

    void GenerateWave()
    {
        _currentWave.isLunched = true;
        foreach (Enemy enemy in waves[0].enemies)
        {
            if (_currentSpawned < _currentWave.enemies.Count)
            {
                GiveRandomLocation();
                float x = _player.position.x + _dirrection.x;
                float y = _player.position.y;
                float z = _player.position.z + _dirrection.z;

                Enemy _enemy= Instantiate(enemy.gameObject, new Vector3(x, y, z), Quaternion.identity).GetComponent<Enemy>();
                _enemiesCopy.Add(_enemy);
                _currentSpawned++;
            }
            else
                break;
        }
    }

    void GiveRandomLocation()
    {
        _dirrection.x = (int)Random.Range(-1, 2) * _currentWave.maxdistance;
        _dirrection.y = (int)Random.Range(-1, 2) * _currentWave.maxdistance;
        if (_dirrection.x == 0 && _dirrection.z == 0)
        {
            _dirrection.x = _currentWave.minimumdistance;
            _dirrection.z = _currentWave.minimumdistance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && _player==null)
        {
            _player = other.transform;
            GenerateWave();
        }
        
    }
}
