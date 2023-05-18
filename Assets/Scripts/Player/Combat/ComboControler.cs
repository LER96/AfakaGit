using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboControler : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] Collider _collider;
    [SerializeField] GameObject _particleVFX_Sword;
    [SerializeField] public bool canMove;
    private int _attackNumber;
    private bool _canAttack;

    private void Awake()
    {
         canMove = true;
        _canAttack = true;
        _particleVFX_Sword.SetActive(false);
    }

    public void StartCombo()
    {
        _animator.SetBool("DisableTransition", false);

        if (_attackNumber < 2)
        {
            _attackNumber++;
        }
        else
        {
            _attackNumber = 0;
        }
    }

    public void FinishAnimation()
    {
        _attackNumber = 0;
        _particleVFX_Sword.SetActive(false);
        canMove = true;
    }

    public void AttackCombo()
    {
        if (_canAttack)
        {
            _animator.SetTrigger("Attack" + _attackNumber);
            _canAttack = false;
            canMove = false;
            Debug.Log(_attackNumber);
        }
    }

    public void CanAttack()
    {
        _canAttack = true;
    }

    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    public void DisableCollider()
    {
        _collider.enabled = false;
    }
}