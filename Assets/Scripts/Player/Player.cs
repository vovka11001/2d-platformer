using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ItemDetector _itemDetector;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private PlayerAttacker _playerAttacker;

    private float _deathCooldown = 1f;
    private List<Coin> _coinsCollected;
    private List<Enemy> _currentEnemies;

    private Rigidbody2D _rigidbody2d;
    private Coroutine _deathCooldownCoroutine;

    public bool IsDead { get; private set; } = false;
    public int Health { get; private set; } = 100;

    private void OnEnable()
    {
        _itemDetector.TriggerEntered += Collect;
        _enemyDetector.TriggerEntered += AddEnemy;
        _enemyDetector.TriggerExited += RemoveEnemy;
        _inputReader.Jumped += Jump;
        _playerAttacker.Attacked += PlayAttackAnimation;
    }

    private void OnDisable()
    {
        _itemDetector.TriggerEntered -= Collect;
        _enemyDetector.TriggerEntered -= AddEnemy;
        _enemyDetector.TriggerExited -= RemoveEnemy;
        _inputReader.Jumped -= Jump;
        _playerAttacker.Attacked -= PlayAttackAnimation;
    }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _currentEnemies = new List<Enemy>();
    }

    private void Start()
    {
        transform.position = _spawnPoint.transform.position;
        _coinsCollected = new List<Coin>();
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (_inputReader.HorizontalInput > 0)
                _rotator.FaceLeft();
            else if (_inputReader.HorizontalInput < 0)
                _rotator.FaceRight();

            bool isGrounded = IsGrounded();
            bool shouldRun = Mathf.Abs(_inputReader.HorizontalInput) > 0 && isGrounded;

            _animationController.SetAnimationRun(shouldRun);
            _animationController.SetGrounded(isGrounded);
        }
    }

    private void FixedUpdate()
    {
        if (!IsDead)
            _rigidbody2d.velocity = new Vector2(_inputReader.HorizontalInput * _speed, _rigidbody2d.velocity.y);
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        Health -= damage;
        _animationController.SetAnimationHurt();

        if (Health <= 0)
        {
            Die();
            _animationController.SetAnimationDie();
        }
    }

    private void PlayAttackAnimation()
    {
        _animationController.SetAnimationAttacking();
    }

    private void Jump()
    {
        if (IsGrounded() && !IsDead)
        {
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _jumpForce);
        }
    }

    private bool IsGrounded()
    {
        if (IsDead)
            return false;

        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);
        return hit.collider != null;
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

    private void Die()
    {
        if (IsDead)
            return;

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

    private void Collect(Item item)
    {
        if (item.TryGetComponent(out Coin coin))
        {
            _coinsCollected.Add(coin);
            coin.Collect();
        }
        else if (item.TryGetComponent(out MedicineChest medicineChest))
        {
            Health += medicineChest.IncreaseAmount;
            medicineChest.Collect();
        }
    }
}