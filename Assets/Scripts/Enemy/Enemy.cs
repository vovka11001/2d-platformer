using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyAttacker _enemyAttacker;
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private EnemyPatrol _enemyPatrol;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private AnimatorEventHandler _animatorEventHandler;
    
    private float _deathCooldown = 1.5f;

    private Coroutine _deathCooldownCoroutine;

    public int Health { get; private set; } = 100;
    public bool IsDead { get; private set; }

    private void OnEnable()
    {
        _animatorEventHandler.Attacked += _enemyAttacker.Attack;
        _enemyAttacker.AttackRequested += _animationController.SetAnimationAttacking;
    }

    private void OnDisable()
    {
        _animatorEventHandler.Attacked -= _enemyAttacker.Attack;
        _enemyAttacker.AttackRequested -= _animationController.SetAnimationAttacking;
    }

    private void Update()
    {
        bool isGrounded = _groundDetector.IsGrounded();
        
        _animationController.SetGrounded(isGrounded);
        _animationController.SetAnimationRun(_enemyMover.IsMoving);

        if (!IsDead)
        {
            if (_enemyPatrol.LookDirection.x > 0)
                _enemyPatrol.RotateLeft();
            else if (_enemyPatrol.LookDirection.x < 0)
                _enemyPatrol.RotateRight();
        }

        _enemyMover.SetTarget(_enemyPatrol.TargetTransform);
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

        _enemyMover.SetMovingFalse();
        _enemyAttacker.SetAttackFalse();
        
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