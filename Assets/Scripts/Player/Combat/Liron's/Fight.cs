using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider _swordCollider;
    [SerializeField] GameObject _particleVFX_Sword;

    [Header("Movment")]
    [SerializeField] public bool canMove;

    [Header("Attacks")]
    [SerializeField] bool baseAttacking;
    [SerializeField] bool heavyAttacking;
    [SerializeField] int baseAttackState;
    [SerializeField] PlayerBehavior player;
    int _baseAttack;


    private void Awake()
    {
        _swordCollider.enabled = false;
        canMove = true;
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerBehavior>();
    }
    private void Update()
    {
        if (baseAttacking == false)
        {
            animator.SetBool("HeavyAttack", heavyAttacking);
        }
        animator.SetInteger("BaseAttack", _baseAttack);

    }

    //Base Attack
    public void BaseAttackAnimation()
    {
        if (baseAttacking == false)
        {
            player.Damage = player.lightAttackDamage;
            baseAttackState++;
            if (baseAttackState > 2)
            {
                baseAttackState = 1;
            }
            _baseAttack = baseAttackState;
            baseAttacking = true;
            canMove = false;
        }
    }


    //Heavy Attack
    public void HeavyAttackAnimation()
    {
        if (baseAttacking == false)
        {
            player.Damage = player.heavyAttackDamage;
            heavyAttacking = true;
            canMove = false;
        }
    }

    //Ivoke Fuctions
    public void EnableCollider()
    {
        _swordCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _swordCollider.enabled = false;
        _particleVFX_Sword.SetActive(false);
    }

    public void BaseAttackAgain()
    {
        _baseAttack = 0;
        baseAttacking = false;
        canMove = true;
    }

    public void HeavyAttackAgain()
    {
        heavyAttacking = false;
        canMove = true;
    }
}