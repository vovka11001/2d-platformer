using System;
using System.Collections;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour, IAttacker
{
    [SerializeField] private PlayerDetector _playerDetector;

    private float _attackCooldown = 2f;

    private Player _currentTarget;
    private Coroutine _attackCooldownCoroutine;

    public event Action Attacked;

    public bool IsAttack { get; private set; }
    public int Damage { get; private set; } = 20;
    
    private void OnEnable()
    {
        _playerDetector.TriggerEntered += OnPlayerEntered;
        _playerDetector.TriggerExited += PlayerExited;
    }

    private void OnDisable()
    {
        _playerDetector.TriggerEntered -= OnPlayerEntered;
        _playerDetector.TriggerExited -= PlayerExited;

        if (_attackCooldownCoroutine != null)
            StopCoroutine(_attackCooldownCoroutine);
    }

    private void Update()
    {
        if (_currentTarget == null || _currentTarget.IsDead)
            return;

        if (IsAttack && _playerDetector.IsOnTriggerEntered)
            Attack(_currentTarget);
    }

    public void SetAttackFalse()
    {
        IsAttack = false;
    }

    public void Attack(IDamageable target)
    {
        if (target == null || target.IsDead) 
            return;

        target.TakeDamage(Damage);
        Attacked?.Invoke();

        IsAttack = false;

        if (_attackCooldownCoroutine != null)
            StopCoroutine(_attackCooldownCoroutine);

        _attackCooldownCoroutine = StartCoroutine(AttackCooldown());
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        IsAttack = true;
        _attackCooldownCoroutine = null;

        if (_currentTarget != null && !_currentTarget.IsDead && _playerDetector.IsOnTriggerEntered)
            Attack(_currentTarget);
    }

    private void OnPlayerEntered(Player player)
    {
        _currentTarget = player;
        IsAttack = true;
    }

    private void PlayerExited(Player player)
    {
        if (_currentTarget == player)
        {
            _currentTarget = null;
            IsAttack = false;

            if (_attackCooldownCoroutine != null)
            {
                StopCoroutine(_attackCooldownCoroutine);
                _attackCooldownCoroutine = null;
            }
        }
    }
}