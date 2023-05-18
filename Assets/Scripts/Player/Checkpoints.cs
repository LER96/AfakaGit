using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] List<GameObject> _checkPoints;
    [SerializeField] Vector3 _vectorPoint;
    [SerializeField] PlayerBehavior _playerBehavior;

    private void Start()
    {
        _playerBehavior = GetComponent<PlayerBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Checkpoint")
        {
            _vectorPoint = other.transform.position;
            Debug.Log("check");
        }
    }

    public void Respawn()
    {
         _player.transform.position = _vectorPoint;
        _playerBehavior.GetBase();
    }
}
