using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slashes : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] Animator animator;
    [SerializeField] bool isAttack;
    [SerializeField] Collider _collider;
    [SerializeField] GameObject _particleVFX_Sword;
    [SerializeField] float counter;
    [SerializeField] float _startTime;

    private void Update()
    {
        if (_startTime > 0)
        {
            float _t = Time.time;
            if (Time.time < _startTime + duration)
            {
                animator.SetBool("Attack", true);
            }
            else
            {
                animator.SetBool("Attack", false);
                _startTime = 0;
            }
        }
    }
    public void Attack()
    {
        if (_startTime == 0)
        {
            _startTime = Time.deltaTime;
        }
    }
}
