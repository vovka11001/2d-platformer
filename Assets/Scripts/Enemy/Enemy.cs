using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private EnemyMover _enemyMover;
    
    private int _health;
    private float _deathCooldown = 1.5f; 
    private float _attackCooldown = 1f;
    
    private bool _canAttack = true;

    private Coroutine _DeathCooldownCoroutine;
    private Coroutine _attackCooldownCoroutine;
    
    public int Damage {get; private set;}
    public bool IsDead { get; private set; }
    public bool IsAttacking { get; private set; }
    
    public event Action Hurt;
    public event Action Death;
    public event Action Attacking;
    public event Action<Player> PlayerExit;
    public event Action<Player> PlayerEntered;

    private void OnEnable()
    {
        _playerDetector.TriggerEntered += OnPlayerEntered;
        _playerDetector.TriggerExited += OnPlayerExited;
    }

    private void OnDisable()
    {
        _playerDetector.TriggerEntered -= OnPlayerEntered;
        _playerDetector.TriggerExited -= OnPlayerExited;
    }
    
    private void Start()
    {
        _health = 100;
        Damage = 10;
    }
    
    public void DecreaseHealth(int damage)
    {
        if (IsDead)
            return;
        
        _health -= damage;
        Hurt?.Invoke();
        
        if (_health <= 0)
        {
            DestroyEnemy();
        }
    }
    
    private void DestroyEnemy()
    {
        if (IsDead) 
            return;
        
        Death?.Invoke();
        IsDead = true;
        
        if (_DeathCooldownCoroutine != null)
            StopCoroutine(_DeathCooldownCoroutine);
        
        _DeathCooldownCoroutine = StartCoroutine(DeathCooldown());
    }
    
    private void Attack(Player player)
    {
        if (!IsDead)
        {
            Attacking?.Invoke();

            if (!_canAttack) 
                return;
        
            _canAttack = false;
        
            if(_playerDetector.IsOnTriggerEntered)
                player.DecreaseHealth(Damage);
        
            if (_attackCooldownCoroutine != null)
                StopCoroutine(_attackCooldownCoroutine);
        
            _attackCooldownCoroutine = StartCoroutine(AttackCooldown());
        }
    }
    
    private void OnPlayerEntered(Player player)
    {
        IsAttacking = true;
        
        if (_canAttack)
            Attack(player);
        
        PlayerEntered?.Invoke(player);
    }
    
    private void OnPlayerExited(Player player)
    {
        IsAttacking = false;
        PlayerExit?.Invoke(player);
    }
    
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        _attackCooldownCoroutine = null;
        
        if (IsAttacking && _enemyMover.PlayerTarget != null)
        {
            Attack(_enemyMover.PlayerTarget);
        }
    }
    
    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(_deathCooldown);
        
        Destroy(gameObject);
    }
}