using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimation : Character
{
    [SerializeField] Animator anim;
    public LayerMask enemyMask;
    private bool _firstAttack = false;
    private bool _secondAttack;
   
    private void Update()
    {
        if (Input.GetMouseButton(0) && !_firstAttack)
        {
            anim.SetTrigger("FirstSlash");
            _firstAttack = true;
        }   
    }

    public override void ApplyDamage(Character enemy)
    {
        base.ApplyDamage(enemy);
    }

    public override void TakeDamage(Character enemy)
    {
        base.TakeDamage(enemy);
    }

    public void BaseAttack()
    {
        this.AttackRange = 2;
        this.Damage = 10;
        anim.SetTrigger("FirstSlash");
    }

    public void HeavyAttack()
    {
        this.AttackRange = 3;
        this.Damage = 15;
    }
}
