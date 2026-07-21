using System;
using System.Collections;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour, IAttacker
{
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private InputReader _inputReader;

    private bool _canAttack = true;
    private int _damage = 20;

    private Enemy _currentTarget;
    private Coroutine _attackCooldownCoroutine;

    public event Action AttackRequested;

    public int Damage => _damage;
    
    private void OnEnable()
    {
        _inputReader.Attacked += RequestAttack;
        _enemyDetector.TriggerEntered += EnemyEntered;
        _enemyDetector.TriggerExited += EnemyExited;
    }

    private void OnDisable()
    {
        _inputReader.Attacked += RequestAttack;
        _enemyDetector.TriggerEntered -= EnemyEntered;
        _enemyDetector.TriggerExited -= EnemyExited;
    }

    public void Attack()
    {
        if (_currentTarget == null || _currentTarget.IsDead) 
            return;
        
        _currentTarget.TakeDamage(Damage);
    }
    
    private void RequestAttack()
    {
        if (!_canAttack)
            return;
        
        _canAttack = false;
        
        AttackRequested?.Invoke();
        
        if (_attackCooldownCoroutine != null)
            StopCoroutine(_attackCooldownCoroutine);
        
        _attackCooldownCoroutine = StartCoroutine(AttackCooldown());
    }
    
    private void EnemyEntered(Enemy enemy)
    {
        _currentTarget = enemy;
    }

    private void EnemyExited(Enemy enemy)
    {
        if (_currentTarget == enemy)
            _currentTarget = null;
    }

    public void SetAttackFalse()
    {
        if (_attackCooldownCoroutine != null)
        {
            StopCoroutine(_attackCooldownCoroutine);
            _attackCooldownCoroutine = null;
        }
        
        _canAttack = false;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        _attackCooldownCoroutine = null;
    }
}