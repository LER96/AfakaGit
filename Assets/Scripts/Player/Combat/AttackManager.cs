using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    [SerializeField] float _knockStrength;
    List<Enemy> cachedEnemies = new List<Enemy>();

    private void OnTriggerEnter(Collider other)
    {
        PlayerBehavior playerBehave = GetComponentInParent<PlayerBehavior>();

        if (other.transform.tag == "RangedEnemy" || other.transform.tag == "MeleeEnemy")
        {
            Enemy enemyCharacter = other.transform.GetComponent<Enemy>();

            if (!cachedEnemies.Contains(enemyCharacter))
            {
                cachedEnemies.Add(enemyCharacter);
                playerBehave.ApplyDamage(enemyCharacter);
                enemyCharacter.ColorFlash();
                enemyCharacter.PlaySplashBlood();
                KnockBack(enemyCharacter.transform);
            }
        }
    }

    public void ClearCachedEnemies()
    {
        cachedEnemies.Clear();
    }

    public void KnockBack(Transform enemy)
    {
        Vector3 hitDirrection = this.transform.position - enemy.transform.position;
        enemy.transform.position -= hitDirrection.normalized * _knockStrength * 10f * Time.deltaTime;
    }
}
