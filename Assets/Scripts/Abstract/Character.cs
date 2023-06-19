using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Header("Character")]
    public float HP;
    public float maxHp;
    public float Damage;
    public float AttackSpeed;
    public float AttackRange;
    public float momentumPoints;
    public bool IsDead;
    public bool IsAttacking;

    public virtual void ApplyDamage(Character enemy)
    {
        enemy.TakeDamage(this);
    }

    public virtual void TakeDamage(Character enemy)
    {
        HP -= enemy.Damage;

        if (HP <= 0)
        {
            HP = 0;
            DeathState(true);
        }
    }

    public virtual void DeathState(bool dead)
    {
        IsDead = dead;

        if(IsDead==false)
        {
            this.gameObject.SetActive(true);
        }
        else
            this.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(float num)
    {
        HP -= num;

        if (HP <= 0)
        {
            HP = 0;
            DeathState(true);
        }
    }
}
