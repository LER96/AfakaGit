using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public bool isEnemyHit = false;
    [SerializeField] float _knockStrength;

    private void OnTriggerEnter(Collider other)
    {
        PlayerBehavior playerBehave = GetComponentInParent<PlayerBehavior>();

        if (other.transform.tag == "RangedEnemy" || other.transform.tag == "MeleeEnemy")
        {
            Debug.Log("hit");
            isEnemyHit = true;
            Enemy enemyCharacter = other.transform.GetComponent<Enemy>();
            playerBehave.ApplyDamage(enemyCharacter);
            enemyCharacter.ColorFlash();
            enemyCharacter.PlaySplashBlood();
            KnockBack(enemyCharacter.transform);
        }
    }

    public void KnockBack(Transform enemy)
    {
        Vector3 hitDirrection = this.transform.position - enemy.transform.position;
        enemy.transform.position -= hitDirrection.normalized*_knockStrength*10f*Time.deltaTime;
    }
}
