using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ItemDetector _itemDetector;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private AnimationController _animationController;
    [SerializeField] private AnimatorEventHandler _animatorEventHandler;
    [SerializeField] private PlayerAttacker _playerAttacker;
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerJump _playerJump;
    [SerializeField] private GroundDetector _groundDetector;
    
    private float _speed = 5f;
    private float _jumpForce = 10f;
    private float _deathCooldown = 1f;
    
    private List<Coin> _coinsCollected;
    private Coroutine _deathCooldownCoroutine;

    public bool IsDead { get; private set; }
    public int Health { get; private set; } = 100;

    private void OnEnable()
    {
        _itemDetector.TriggerEntered += Collect;
        _inputReader.Jumped += Jump;
        _playerAttacker.AttackRequested += PlayAttackAnimation;
        _animatorEventHandler.Attacked += _playerAttacker.Attack;
    }

    private void OnDisable()
    {
        _itemDetector.TriggerEntered -= Collect;
        _inputReader.Jumped -= Jump;
        _playerAttacker.AttackRequested -= PlayAttackAnimation;
        _animatorEventHandler.Attacked -= _playerAttacker.Attack;
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

            bool isGrounded = _groundDetector.IsGrounded();
            bool shouldRun = Mathf.Abs(_inputReader.HorizontalInput) > 0 && isGrounded;

            _animationController.SetAnimationRun(shouldRun);
            _animationController.SetGrounded(isGrounded);
        }
    }

    private void FixedUpdate()
    {
        if (!IsDead)
            _playerMover.Run(_inputReader.HorizontalInput * _speed);
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
        if (!IsDead)
            _animationController.SetAnimationAttacking();
    }

    private void Jump()
    {
        if (_groundDetector.IsGrounded() && !IsDead)
            _playerJump.Jump(_jumpForce);
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
        _playerAttacker.SetAttackFalse();

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