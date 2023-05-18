using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider _collider;
    [SerializeField] GameObject _particleVFX_Sword;
    [SerializeField] public bool canMove;
    [SerializeField] bool attacking;
    [SerializeField] float delay;
    float currentDelay;

    [Header("Heavy Attack")]
    [SerializeField] PlayerBehavior player;
    [SerializeField] float currentCharge;
    [SerializeField] float chargeMultiplier=5;
    [SerializeField] bool isReleased;
    PlayerBehavior _playerStates;
    private void Awake()
    {
        _playerStates = player.GetComponent<PlayerBehavior>();
        _collider.enabled = false;
        isReleased = true;
    }
    private void Update()
    {
        animator.SetFloat("Charge", currentCharge);
        animator.SetBool("BaseAttack", attacking);
        animator.SetBool("Release", isReleased);
        HeavyAttack();
        BaseAttack();
    }
    public void BaseAttackAnimation()
    {
        currentDelay = delay;
        attacking = true;
        canMove = false;
    }
    void BaseAttack()
    {
        if (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
            _playerStates.Damage = _playerStates.lightAttackDamage;
            player.Damage = _playerStates.Damage;
        }
        else if(isReleased)
        {
            currentDelay = 0;
            attacking = false;
            _particleVFX_Sword.SetActive(false);
            canMove = true;
        }
    }
    void HeavyAttack()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isReleased = false;
            canMove = false;
            if (currentCharge >= 1 && _playerStates.Damage>=_playerStates.heavyAttackDamage)
            {
                currentCharge = 1;
                _playerStates.Damage = _playerStates.heavyAttackDamage;
            }
            else
            {
                currentCharge += Time.deltaTime * chargeMultiplier;
                _playerStates.Damage = currentCharge * _playerStates.heavyAttackDamage;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            currentCharge = 0;
            isReleased = true;
            player.Damage = _playerStates.Damage;
        }
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