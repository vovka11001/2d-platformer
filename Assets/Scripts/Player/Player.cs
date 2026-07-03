using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ItemDetector _itemDetector;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Player _player;

    private bool _canAttack = true;
    private float _deathCooldown = 1f;
    private List<Coin> _coinsCollected;
    private List<Enemy> _currentEnemies;

    private Rigidbody2D _rigidbody2d;
    private Coroutine _deathCooldownCoroutine;
    private Coroutine _attackCooldownCoroutine;

    public bool IsDead { get; private set; }
    public int Damage { get; private set; }    
    public int Health { get; private set; }    
    
    public event Action DecreasedHealth;
    public event Action Death;

    private void OnEnable()
    {
        _itemDetector.TriggerEntered += Collect;
        _enemyDetector.TriggerEntered += AddEnemy;
        _enemyDetector.TriggerExited += RemoveEnemy;
        _inputReader.Attacked += Attack;
        _inputReader.Jumped += Jump;
        _player.DecreasedHealth += SetAnimationHurt;
        _player.Death += SetAnimationDeath;
    }

    private void OnDisable()
    {
        _itemDetector.TriggerEntered -= Collect;
        _enemyDetector.TriggerEntered -= AddEnemy;
        _enemyDetector.TriggerExited -= RemoveEnemy;
        _inputReader.Attacked -= Attack;
        _inputReader.Jumped -= Jump;
        _player.DecreasedHealth -= SetAnimationHurt;
        _player.Death -= SetAnimationDeath;
    }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _currentEnemies = new List<Enemy>();
    }

    private void Start()
    {
        Health = 100;
        Damage = 25;
        transform.position = _spawnPoint.transform.position;
        _coinsCollected = new List<Coin>();
    }

    private void Update()
    {
        if (!_player.IsDead)
        {
            if (_inputReader.HorizontalInput > 0)
                _rotator.FaceLeft();
            else if (_inputReader.HorizontalInput < 0)
                _rotator.FaceRight();

            bool isGrounded = IsGrounded();
            bool shouldRun = Mathf.Abs(_inputReader.HorizontalInput) > 0 && isGrounded;

            _animator.SetBool(AnimatorData.Params.IsRunning, shouldRun);
            _animator.SetBool(AnimatorData.Params.Grounded, isGrounded);
        }
    }

    private void FixedUpdate()
    {
        if (!_player.IsDead)
            _rigidbody2d.velocity = new Vector2(_inputReader.HorizontalInput * _speed, _rigidbody2d.velocity.y);
    }

    public void DecreaseHealth(int damage)
    {
        if (IsDead)
            return;

        if (!IsDead)
        {
            Health -= damage;
            DecreasedHealth?.Invoke();
        }

        if (Health <= 0)
        {
            DestroyPlayer();
        }
    }

    private void Jump()
    {
        if (IsGrounded() && !_player.IsDead)
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _jumpForce);
    }

    private void SetAnimationHurt()
    {
        _animator.SetBool(AnimatorData.Params.Hurt, true);
    }

    private void SetAnimationDeath()
    {
        _animator.SetBool(AnimatorData.Params.Death, true);
    }

    private void Attack()
    {
        if (!_player.IsDead)
        {
            if (!_canAttack)
                return;

            _canAttack = false;
            _animator.SetBool(AnimatorData.Params.Attack, true);

            if (_currentEnemies.Count > 0)
            {
                _currentEnemies.RemoveAll(enemy => enemy == null);

                foreach (var enemy in _currentEnemies)
                {
                    if (enemy != null)
                    {
                        enemy.DecreaseHealth(_player.Damage);
                    }
                }
            }

            if (_attackCooldownCoroutine != null)
                StopCoroutine(_attackCooldownCoroutine);

            _attackCooldownCoroutine = StartCoroutine(AttackCooldown());
        }
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        _attackCooldownCoroutine = null;
    }

    private bool IsGrounded()
    {
        if (!_player.IsDead)
        {
            float distance = 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);
            return hit.collider != null;
        }

        return false;
    }

    private void AddEnemy(Enemy enemy)
    {
        _currentEnemies.Add(enemy);
    }

    private void RemoveEnemy(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);
    }
    

    private void DestroyPlayer()
    {
        if (IsDead) 
            return;
        
        Death?.Invoke();
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
            item.Collect();
        }
        else if (item.TryGetComponent(out MedicineChest medicineChest))
        {
            Health += medicineChest.IncreaseAmount;
            medicineChest.Collect();
        }
    }
}