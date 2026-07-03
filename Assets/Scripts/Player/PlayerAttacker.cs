using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public class PlayerAttacker : MonoBehaviour, IAttacker
{
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Player _player;

    private bool _canAttack = true;
    private int _damage = 20;

    private List<IDamageable> _currentEnemies = new List<IDamageable>();
    private Coroutine _attackCooldownCoroutine;

    public int Damage => _damage;

    private void OnEnable()
    {
        _enemyDetector.TriggerEntered += AddEnemy;
        _enemyDetector.TriggerExited += RemoveEnemy;
    }

    private void OnDisable()
    {
        _enemyDetector.TriggerEntered -= AddEnemy;
        _enemyDetector.TriggerExited -= RemoveEnemy;
    }

    public void PerformAttack()
    {
        if (!_canAttack || _player.IsDead)
            return;

        _canAttack = false;

        _currentEnemies.RemoveAll(enemy => enemy == null || enemy.IsDead);

        foreach (var enemy in _currentEnemies)
        {
            if (enemy != null && !enemy.IsDead)
            {
                Attack(enemy);
            }
        }

        if (_attackCooldownCoroutine != null)
            StopCoroutine(_attackCooldownCoroutine);

        _attackCooldownCoroutine = StartCoroutine(AttackCooldown());
    }

    public void Attack(IDamageable target)
    {
        if (_player.IsDead || target == null || target.IsDead) 
            return;

        target.TakeDamage(Damage);
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        _attackCooldownCoroutine = null;
    }

    private void AddEnemy(Enemy enemy)
    {
        if (enemy != null && !_currentEnemies.Contains(enemy))
            _currentEnemies.Add(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        if (_currentEnemies.Contains(enemy))
            _currentEnemies.Remove(enemy);
    }
}
