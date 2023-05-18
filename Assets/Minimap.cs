using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform _player;

    private void LateUpdate()
    {
        Vector3 newPositon = _player.position;
        newPositon.y = transform.position.y;
        transform.position = newPositon;
    }
}
