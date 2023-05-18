using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Enemy : Character
{
    [SerializeField] Material[] _material;
    [SerializeField] public MeshRenderer _meshRenderer;
    [SerializeField] public VisualEffect visualEffect;
    [SerializeField] Color _originalColor;
    [SerializeField] float _flashTime = 2f;

    [Header("Attack Variables")]
    public float SightRange;
    [SerializeField] private LayerMask _playerMask;
    [SerializeField] private float _attackRate = 2f;
    private float _nextAttackTime = 0f;
    private bool _attacked;
    private bool _playerInAttackRange;

    [Header("Shoot")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] GameObject shootSpot;
    public NavMeshAgent agent;
    public GameObject player;
    private Character playerScript;
    Projectile _proj;

    [SerializeField] Image _healthBar;
    [SerializeField] Canvas _HealthCanvas;
    [SerializeField] Camera _camera;

    public void Start()
    {
        playerScript = player.GetComponent<Character>();
        _meshRenderer.sharedMaterial = _material[0];
        _camera = Camera.main;
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
        _meshRenderer.sharedMaterial = _material[1];
        Invoke("ResetColor", _flashTime * Time.fixedDeltaTime);
    }

    void ResetColor()
    {
        _meshRenderer.sharedMaterial = _material[0];
    }

    public void CheckSight()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= SightRange)
        {
            Vector3 dirrect = player.transform.position - transform.position;
            Vector3 newDirrect = Vector3.RotateTowards(transform.forward, dirrect, 3*Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newDirrect);
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
        //if (Time.time >= _nextAttackTime)
        //{
        //    if (Vector3.Distance(transform.position, player.transform.position) <= this.AttackRange)
        //    {
        //        this.ApplyDamage(playerScript);
        //        _nextAttackTime = Time.time + _attackRate;
        //    }
        //}
    }

    public void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        shootSpot.transform.LookAt(player.transform.position);

        if (!_attacked && IsDead==false)
        {
            GameObject proj = Instantiate(projectilePrefab, shootSpot.transform.position, shootSpot.transform.rotation);
            Projectile projectile = proj.GetComponent<Projectile>();
            projectile.SetFather(this);

            _attacked = true;
            Invoke(nameof(ResetAttack), _attackRate);
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
    }
}
