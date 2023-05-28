using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;
using UnityEngine.UI;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class PlayerBehavior : Character
{
    [Header("Damage Variables")]
    [SerializeField] public float lightAttackDamage;
    [SerializeField] public float heavyAttackDamage;

    [Header("Floats")]
    [SerializeField] float maxHP;
    [SerializeField] float _maxMomentumPoints;
    [SerializeField] float _momentumFillAmount;

    [Header("Increase States By")]
    [Range(0, 6)]
    [SerializeField] float firstBuffMultiplier;
    [Range(0, 6)]
    [SerializeField] float secondBuffMultiplier;
    [Range(0, 6)]
    [SerializeField] float ultimateBuffMultiplier;

    [SerializeField] float _damageIncrease = 10;
    [SerializeField] float _healthIncrease = 20;
    [SerializeField] float _movementSpeedIncrease = 2;

    [Header("Buff Progress")]
    [Range(0, 1)]
    [SerializeField] float firstBuff;
    [Range(0, 1)]
    [SerializeField] float secondBuff;
    [Range(0, 1)]
    [SerializeField] float ultimateBuff;

    [Header("Momentum CoolDown")]
    [HideInInspector] public bool _coolDownMomentum;
    [SerializeField] float _coolDownMomentumTime;
    [SerializeField] float _currentCoolDownTime;
    [SerializeField] float decreaseMomentum;

    [Header("References")]
    [SerializeField] Image _momentumBar;
    [SerializeField] Image _healthBar;
    [SerializeField] GameObject _fullMomentumBar;
    [SerializeField] ParticleSystem _swordVFX;
    [SerializeField] CharacterController _characterController;
    [SerializeField] Enemy _enemyHealthBar;

    Animator _animator;
    PlayerMovement _playerMovement;
    public LayerMask enemyMask;

    MomentumState _momentumState;
    MomentumState momentumCopy;

    PlayerStatus _playerOriginal;
    PlayerStatus _playerBuffState = new PlayerStatus();

    public enum MomentumState
    {
        None,
        firstBuff,
        secondBuff,
        lastBuff,
    }

    private void Start()
    {
        SetBase();
        this.maxHP = HP;
        _momentumState = MomentumState.None;
        momentumCopy = _momentumState;
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        MomentumVFXChange(new Color(207f / 255f, 101f / 255f, 36f / 255f), 0.06f, 0.7f);
    }

    private void Update()
    {
        HealthBarFill();

        if (_coolDownMomentum)
            CoolDownMomentum();
        else
        {
            if (momentumPoints > 0)
            {
                momentumPoints -= decreaseMomentum * Time.deltaTime;
                GetCurrentMomentumFill();
            }
            else
                momentumPoints = 0;
        }
    }

    public override void ApplyDamage(Character enemy)
    {
        base.ApplyDamage(enemy);
        Debug.Log($"{enemy.HP} + {enemy.maxHp}");
        OnMomemntumChange(enemy.momentumPoints);
    }

    public override void TakeDamage(Character enemy)
    {
        HP -= enemy.Damage;

        if (HP < 0)
        {
            HP = 0;
            _animator.SetBool("Dead", true);
            //DeathState(true);
        }
    }

    public void Dead()
    {
        DeathState(true);
    }

    public override void DeathState(bool dead)
    {
        base.DeathState(dead);
    }

    public void BaseAttack(float dmg)
    {
        this.Damage = dmg;
    }

    public void HeavyAttack(float dmg)
    {
        this.Damage = dmg;
    }

    public void OnMomemntumChange(float moment)
    {
        if (this.momentumPoints < _maxMomentumPoints)
            this.momentumPoints += moment;
        else
            this.momentumPoints = _maxMomentumPoints;

        _coolDownMomentum = true;
        GetCurrentMomentumFill();
    }

    void CoolDownMomentum()
    {
        if (_currentCoolDownTime >= _coolDownMomentumTime)
        {
            _currentCoolDownTime = 0;
            _coolDownMomentum = false;
        }
        else
        {
            _currentCoolDownTime += Time.deltaTime;
        }
    }

    void GetCurrentMomentumFill()
    {
        _momentumFillAmount = this.momentumPoints / _maxMomentumPoints;
        _momentumBar.fillAmount = _momentumFillAmount;
        CheckMomentum();
    }

    void HealthBarFill()
    {
        float fillAmount = this.HP / this.maxHP;
        _healthBar.fillAmount = fillAmount;
    }

    void CheckMomentum()
    {
        if (_momentumFillAmount >= firstBuff && _momentumFillAmount < secondBuff)
        {
            _momentumState = MomentumState.firstBuff;
        }
        else if (_momentumFillAmount >= secondBuff && _momentumFillAmount < ultimateBuff)
        {
            _momentumState = MomentumState.secondBuff;
        }
        else if (_momentumFillAmount >= ultimateBuff)
        {
            _momentumState = MomentumState.lastBuff;
        }
        else if (_momentumFillAmount < firstBuff && _momentumFillAmount >= 0)
        {
            _momentumState = MomentumState.None;
        }

        if (momentumCopy != _momentumState)
        {
            MomentumStateChange();
        }
    }

    public void MomentumStateChange()
    {
        switch (_momentumState)
        {
            case MomentumState.firstBuff:
                momentumCopy = _momentumState;
                MomentumBuff(firstBuffMultiplier);
                _fullMomentumBar.SetActive(false);
                MomentumVFXChange(new Color(207f / 255f, 101f / 255f, 36f / 255f), 0.06f, 0.7f);
                break;
            case MomentumState.secondBuff:
                momentumCopy = _momentumState;
                MomentumBuff(secondBuffMultiplier);
                _fullMomentumBar.SetActive(false);
                break;
            case MomentumState.lastBuff:
                momentumCopy = _momentumState;
                MomentumBuff(ultimateBuffMultiplier);
                _fullMomentumBar.SetActive(true);
                MomentumVFXChange(Color.cyan, 0.5f, 1.1f);
                break;
            case MomentumState.None:
                momentumCopy = _momentumState;
                MomentumBuff(0);
                _fullMomentumBar.SetActive(false);
                MomentumVFXChange(new Color(207f / 255f, 101f / 255f, 36f / 255f), 0.06f, 0.7f);
                break;
        }
    }

    public void MomentumBuff(float multi)
    {
        _playerBuffState.SetPlayerStatus(_playerOriginal, _healthIncrease, _damageIncrease, multi);
        this.lightAttackDamage = _playerBuffState.lightAttackDamage;
        this.heavyAttackDamage = _playerBuffState.heavyAttackDamage;
        this.HP = (this.HP * _playerBuffState.maxHp) / this.maxHP;
        this.maxHP = _playerBuffState.maxHp;
    }

    public void HealthPotion()
    {
        if (InventoryManager.instance.inventory.Count > 0 && this.HP != maxHP)
        {
            InventoryItem item = InventoryManager.instance.inventory[0];
            this.HP += item.data.value;

            InventoryManager.instance.Remove(item.data);
        }
    }

    public void MomentumVFXChange(Color color, float minSize, float maxSize)
    {
        var main = _swordVFX.main;
        main.startColor = color;
        main.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);
    }

    public void ResetStats()
    {
        GetBase();
        DeathState(false);
    }

    void SetBase()
    {
        _playerOriginal = new PlayerStatus(this.HP, this.maxHP, this.lightAttackDamage, this.heavyAttackDamage);
    }

    public void GetBase()
    {
        this.lightAttackDamage = _playerOriginal.lightAttackDamage;
        this.heavyAttackDamage = _playerOriginal.heavyAttackDamage;
        this.maxHP = _playerOriginal.maxHp;
        this.HP = _playerOriginal.HP;
    }

    public void ManuallyChangeMomentum(int num)
    {
        switch (num)
        {
            case 0:
                momentumPoints = 0;
                GetCurrentMomentumFill();
                break;
            case 1:
                momentumPoints = firstBuff * 100;
                GetCurrentMomentumFill();
                break;
            case 2:
                momentumPoints = secondBuff * 100;
                GetCurrentMomentumFill();
                break;
            case 3:
                momentumPoints = ultimateBuff * 100;
                GetCurrentMomentumFill();
                break;

        }
    }
}