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
    [SerializeField] public bool baseAttacking;
    [SerializeField] public bool heavyAttacking;
    [SerializeField] int baseAttackState;
    [SerializeField] PlayerBehavior player;
    public float _heavyAttackCD = 5f;
    public float _heavyAttackCDTimer = 5f;
    int _baseAttack;

    //Layan 3/4/2023
    //private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) => Mathf.Atan2(a.z - b.z, a.x -b.x) * Mathf.Rad2Deg;

    private void Awake()
    {
        _swordCollider.enabled = false;
        canMove = true;
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerBehavior>();
        _heavyAttackCDTimer = _heavyAttackCD;
    }

    private void Update()
    {
      // if (baseAttacking == false)
      // {
      //     animator.SetBool("heavyAttack", heavyAttacking);
      // }

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
        }
    }

    //Heavy Attack
    public void HeavyAttackAnimation()
    {
        if (baseAttacking == false && Time.time > _heavyAttackCD)
        {
             //heavyAttacking = true;
            _particleVFX_Sword.SetActive(true);
            player.Damage = player.heavyAttackDamage;
            animator.SetTrigger("HeavyAttack");
            _heavyAttackCD = Time.time + _heavyAttackCDTimer;
        }
    }

    //Invoke Functions
    public void EnableCollider()
    {
        _swordCollider.enabled = true;
    }

    public void DisableCollider()
    {
        _swordCollider.enabled = false;
        _particleVFX_Sword.SetActive(false);
    }

    //Layan 3/4/2023
    //public void LookAtMouse()
    //{
    //    var yPlane = new Plane(inNormal: transform.up, inPoint: transform.position);
    //    var mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

    //    if (!yPlane.Raycast(mouseRay, out var entered))
    //    {
    //        return;
    //    }

    //    Vector3 point = mouseRay.GetPoint(entered);
    //    Vector3 dirrect = (point - transform.position).normalized;
    //    float angle = Mathf.Atan2(dirrect.x , dirrect.z) * Mathf.Rad2Deg; //AngleBetweenTwoPoints(point, transform.position) * -1;

    //    transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));   
    //}

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