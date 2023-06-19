using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesCollide : MonoBehaviour
{
    [SerializeField] float _minRadius;
    [SerializeField] float _maxRadius;
    [SerializeField] float _currentRadius;
    [SerializeField] float _time;

    [SerializeField] SphereCollider _collider;
    [SerializeField] GameObject _player;


    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "RangedEnemy")
        {
            Enemy _enemy = other.GetComponent<Enemy>();
            _enemy.TakeDamage(22);
        }
    }

    public void IncreaseSize()
    {
        _collider.gameObject.SetActive(true);
        this.transform.SetParent(null);
        StopAllCoroutines();
        StartCoroutine(SizeLoop());
    }

    public IEnumerator SizeLoop()
    {
        float currentTime = 0;
        float increaseBy = (_maxRadius - _currentRadius) / _time; 
        while (currentTime <= _time)
        {
            
            _currentRadius += increaseBy * Time.deltaTime;
            _collider.radius = _currentRadius;
            currentTime += Time.deltaTime;
            yield return null;
        }
        ResetSphere();
    }

    private void ResetSphere()
    {
        _currentRadius = _minRadius;
        _collider.radius = _currentRadius;
        this.transform.SetParent(_player.transform);
        this.transform.localPosition = new Vector3(0,4.8f,0);
        _collider.gameObject.SetActive(false);
    }
}
