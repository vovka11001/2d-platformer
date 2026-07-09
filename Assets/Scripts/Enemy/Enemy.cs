using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyAttacker _enemyAttacker;
    [SerializeField] private EnemyMover _enemyMover;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private GroundDetector _groundDetector;
    
    private float _deathCooldown = 1.5f;

    private Coroutine _deathCooldownCoroutine;

    public int Health { get; private set; } = 100;
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
        bool isGrounded = _groundDetector.IsGrounded();
        
        _animationController.SetGrounded(isGrounded);
        _animationController.SetAnimationRun(_enemyMover.IsMoving);

        if (!IsDead)
        {
            if (_enemyMover.LookDirection.x > 0)
                _enemyMover.RotateLeft();
            else if (_enemyMover.LookDirection.x < 0)
                _enemyMover.RotateRight();
        }
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