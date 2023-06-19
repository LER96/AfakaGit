using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Events;

public class Enemy : Character
{
    [SerializeField] Item _itemDrop;
    public bool _gotHit = false;

    [Header("Visual Effects")]
    [SerializeField] public MeshRenderer meshRenderer;
    [SerializeField] public VisualEffect visualEffect;
    [SerializeField] Material[] _material;
    [SerializeField] float _flashTime = 2f;
    [SerializeField] bool _EnemySawPlayer;
    [SerializeField] Color _originalColor;
    [SerializeField] PlayerBehavior _playerBehavior;
    EnemyType _enemyType;

    [Header("Attack Variables")]
    public float SightRange;
    [SerializeField] LayerMask _playerMask;
    [SerializeField] float _attackRate = 2f;
    float _nextAttackTime = 0f;
    bool _attacked;
    bool _playerInAttackRange;

    [Header("Shoot")]
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject playerShootPoint;
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] GameObject shootSpot;
    Character playerScript;
    Projectile _proj;

    [Header("UI")]
    [SerializeField] Image _healthBar;
    [SerializeField] Canvas _HealthCanvas;
    [SerializeField] Camera _camera;

    public enum EnemyType
    {
        Ranged,
        Melee
    }

    void CheckEnemyType()
    {
        if (this.CompareTag("RangedEnemy"))
        {
            _enemyType = EnemyType.Ranged;
        }
        else if (this.CompareTag("MeleeEnemy"))
        {
            _enemyType = EnemyType.Melee;
        }
    }

    public void Start()
    {
        //player refernces 
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Character>();
        _playerBehavior = player.GetComponent<PlayerBehavior>();
        playerShootPoint = GameObject.FindGameObjectWithTag("ShootPoint");

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.sharedMaterial = _material[0];
        _camera = Camera.main;
        CheckEnemyType();
    }

    public void Update()
    {
        CheckSight();
        InAttackRange();
        AdjustHPBar();

        if (_playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    public void ColorFlash()
    {
        meshRenderer.sharedMaterial = _material[1];
        Invoke("ResetColor", _flashTime * Time.fixedDeltaTime);
    }

    void ResetColor()
    {
        meshRenderer.sharedMaterial = _material[0];
    }

    public void CheckSight()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= SightRange)
        {
            Vector3 dirrect = player.transform.position - transform.position;
            Vector3 newDirrect = Vector3.RotateTowards(transform.forward, dirrect, 3 * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDirrect);
            _EnemySawPlayer = true;
        }

        if (_EnemySawPlayer == true)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public void InAttackRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= this.AttackRange)
        {
            _playerInAttackRange = true;
        }
        else
            _playerInAttackRange = false;
    }

    public virtual void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        shootSpot.transform.LookAt(playerShootPoint.transform.position);

        if (!_attacked && IsDead == false)
        {
            switch (_enemyType)
            {
                case EnemyType.Ranged:
                    GameObject proj = Instantiate(_projectilePrefab, shootSpot.transform.position, shootSpot.transform.rotation);
                    Projectile projectile = proj.GetComponent<Projectile>();
                    projectile.SetFather(this);

                    _attacked = true;
                    Invoke(nameof(ResetAttack), _attackRate);
                    break;
                case EnemyType.Melee:
                    if (Time.time >= _nextAttackTime)
                    {
                        if (Vector3.Distance(transform.position, player.transform.position) <= this.AttackRange)
                        {
                            this.ApplyDamage(playerScript);
                            _nextAttackTime = Time.time + _attackRate;
                        }
                    }
                    break;
            }
        }
    }

    public void ResetAttack()
    {
        _attacked = false;
    }

    public void PlaySplashBlood()
    {
        visualEffect.Play();
    }

    public void UpdateHealthBar()
    {
        _healthBar.fillAmount = this.HP / this.maxHp;
    }

    public void AdjustHPBar()
    {
        _HealthCanvas.transform.rotation = _camera.transform.rotation;
    }

    public override void TakeDamage(Character enemy)
    {
        base.TakeDamage(enemy);
        UpdateHealthBar();

        if (this.IsDead)
        {
            if (Random.Range(0, 100) <= _itemDrop.dropChance)
                Instantiate(_itemDrop.prefab, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), transform.rotation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ultimate")
        {
            Debug.Log("hit enemy");
            TakeDamage(15);
        }
    }
}