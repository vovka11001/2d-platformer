using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyAttacker _enemyAttacker;
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private AnimationController _animationController;

    private float _deathCooldown = 1.5f;

    private Coroutine _deathCooldownCoroutine;

    public int Health { get; private set; } = 100;
    public int Damage { get; private set; } = 20;
    public bool IsDead { get; private set; }

    private void OnEnable()
    {
        _enemyAttacker.Attacked += PlayAnimationAttack;
    }

    private void OnDisable()
    {
        _enemyAttacker.Attacked -= PlayAnimationAttack;
    }

    private void PlayAnimationAttack()
    {
        _animationController.SetAnimationAttacking();
    }

    private void Update()
    {
        _animationController.SetAnimationRun(_enemyMover.IsMoving && !_enemyAttacker.IsAttack);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        Health -= damage;
        _animationController.SetAnimationHurt();

        if (Health <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        if (IsDead)
            return;

        _animationController.SetAnimationDie();
        IsDead = true;

        if (_deathCooldownCoroutine != null)
            StopCoroutine(_deathCooldownCoroutine);

        _deathCooldownCoroutine = StartCoroutine(DeathCooldown());
    }

    private IEnumerator DeathCooldown()
    {
        yield return new WaitForSeconds(_deathCooldown);
        Destroy(gameObject);
    }
}