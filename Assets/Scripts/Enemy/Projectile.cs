using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Enemy _enemy;
    float speed = 3f;
    float projLifeSpan = 5f;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.tag == "Player")
        {
            PlayerBehavior player =  coll.GetComponent<PlayerBehavior>();
            player.TakeDamage(_enemy);
            Destroy(this.gameObject);
        }
    }

    public void SetFather(Enemy enemy)
    {
        _enemy = enemy;
        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        float t = 0;

        while(t < projLifeSpan)
        {
            transform.Translate(new Vector3(0,0,speed*Time.deltaTime));
            t += Time.deltaTime;
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
