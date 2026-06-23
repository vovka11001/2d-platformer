using System.Collections;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxDistance = 0.01f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private Target[] _targets;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private Enemy _enemy;

    private bool _canAttack = true;
    private bool _isAttacking;
    
    private Transform _targetTransform;
    private Player _playerTarget;
    private Rigidbody2D _rigidbody2d;
    private Coroutine _attackCooldownCoroutine;

    public bool IsMoving { get; private set; } = true;
    public Vector2 LookDirection { get; private set; }

    public event UnityAction Runned;
    public event UnityAction StopedMoving;
    public event UnityAction Attacking;

    private void OnEnable()
    {
        _playerDetector.PlayerChanged += UpdatePlayerTarget;
        _playerDetector.TriggerEntered += OnPlayerEntered;
        _playerDetector.TriggerExited += OnPlayerExited;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerChanged -= UpdatePlayerTarget;
        _playerDetector.TriggerEntered -= OnPlayerEntered;
        _playerDetector.TriggerExited -= OnPlayerExited;
    }
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        transform.position = _spawnPoint.position;
        SetTarget();
    }
    
    private void FixedUpdate()
    {
        if (IsMoving && !_isAttacking && !_enemy.IsDead && _targetTransform != null)
        {
            _rigidbody2d.position = Vector2.MoveTowards(_rigidbody2d.position, _targetTransform.position, _speed * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        if (_targetTransform == null) 
            return;
        
        if(_enemy.IsDead)
            return;
        
        if (ReachedTarget())
        {
            SetTarget();
        }

        var offsetVector = (Vector2)_targetTransform.position - _rigidbody2d.position;
        var lookDirection = offsetVector.normalized;
        lookDirection.y = 0;
        LookDirection = lookDirection;
        const float offset = 0.001f;

        if (offsetVector.sqrMagnitude > offset && !_isAttacking)
        {
            Runned?.Invoke();
            if (LookDirection.x > 0)
            {
                _spriteRenderer.flipX = true;
            }
            else if (LookDirection.x < 0)
            {
                _spriteRenderer.flipX = false;
            }
        }
        
        if (_isAttacking && _canAttack)
        {
            Attack(_playerTarget);
        }
    }

    private void UpdatePlayerTarget(Player player)
    {
        _playerTarget = player;
        SetTarget();
    }
    
    private void SetTarget()
    {
        if (_playerTarget != null)
        {
            _targetTransform = _playerTarget.transform;
        }
        else
        {
            int randomTargetIndex = Random.Range(0, _targets.Length);
            _targetTransform = _targets[randomTargetIndex].transform;
        }
    }

    private void OnPlayerEntered(Player player)
    {
        _playerTarget = player;
        _isAttacking = true;
        IsMoving = false;
        StopedMoving?.Invoke();
        
        if (_canAttack)
        {
            Attack(player);
        }
    }
    
    private void OnPlayerExited(Player player)
    {
        _isAttacking = false;
        IsMoving = true;
        
        if (_playerTarget != null && _playerTarget == player)
        {
            _playerTarget = null;
            SetTarget();
        }
    }
    
    private void Attack(Player player)
    {
        if (!_enemy.IsDead)
        {
            Attacking?.Invoke();

            if (!_canAttack) 
                return;
        
            _canAttack = false;
        
            if(_playerDetector.IsOnTriggerEntered)
                player.DecreaseHealth(_enemy.Damage);
        
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
        
        if (_isAttacking && _playerTarget != null)
        {
            Attack(_playerTarget);
        }
    }
    
    private bool ReachedTarget()
    {
        if (_targetTransform == null) return false;
        float distance = Mathf.Abs(transform.position.x - _targetTransform.position.x);
        return distance <= _maxDistance;
    }
}