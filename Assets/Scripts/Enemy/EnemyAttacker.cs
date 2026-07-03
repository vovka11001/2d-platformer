using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacker : MonoBehaviour
{
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private PlayerDetector _playerDetector;

    private float _attackTimer = 0f;
    private Player _currentTarget;

    public int Damage => _enemy.Damage;

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

    private void Update()
    {
        if (_enemy.IsDead || _currentTarget == null || _currentTarget.IsDead) return;

        _attackTimer += Time.deltaTime;

        if (_attackTimer >= _attackCooldown)
        {
            _attackTimer = 0f;

            if (_playerDetector.IsOnTriggerEntered)
            {
                Attack(_currentTarget);
            }
        }
    }

    // Реализация IAttacker
    public void Attack(IDamageable target)
    {
        if (_enemy.IsDead || target == null || target.IsDead) return;

        target.TakeDamage(Damage);
        Debug.Log($"Враг атакует! Урон: {Damage}");
    }

    private void OnPlayerEntered(Player player)
    {
        _currentTarget = player;
        _attackTimer = _attackCooldown; // Мгновенная атака при входе
    }

    private void OnPlayerExited(Player player)
    {
        if (_currentTarget == player)
        {
            _currentTarget = null;
            _attackTimer = 0f;
        }
    }
}
