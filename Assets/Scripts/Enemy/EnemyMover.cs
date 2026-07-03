using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxDistance = 0.01f;
    [SerializeField] private Target[] _targets;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private EnemyAttacker _enemyAttacker;

    private Transform _targetTransform;
    private Rigidbody2D _rigidbody2d;

    public bool IsMoving { get; private set; } = true;
    public Player PlayerTarget {get; private set;}
    public Vector2 LookDirection { get; private set; }

    private void OnEnable()
    {
        _playerDetector.PlayerChanged += UpdatePlayerTarget;
        _playerDetector.TriggerEntered += StopMoving;
        _playerDetector.TriggerExited += StartMoving;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerChanged -= UpdatePlayerTarget;
        _playerDetector.TriggerEntered -= StopMoving;
        _playerDetector.TriggerExited -= StartMoving;
    }
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        transform.position = _spawnPoint.position;
        SetTarget();
    }
    
    private void FixedUpdate()
    {
        if (IsMoving && !_enemyAttacker.IsAttack && !_enemy.IsDead && _targetTransform != null)
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
            SetTarget();

        var offsetVector = (Vector2)_targetTransform.position - _rigidbody2d.position;
        Vector2 lookDirection = offsetVector.normalized;
        lookDirection.y = 0;
        LookDirection = lookDirection;
        const float offset = 0.001f;

        if (LookDirection.x > 0)
            _rotator.FaceLeft(); 

        else if (LookDirection.x < 0)
            _rotator.FaceRight(); 

        if (offsetVector.sqrMagnitude > offset && !_enemyAttacker.IsAttack)
            _rotator.FaceDirection(LookDirection);
    }

    private void UpdatePlayerTarget(Player player)
    {
        PlayerTarget = player;
        SetTarget();
    }
    
    private void SetTarget()
    {
        if (PlayerTarget != null)
        {
            _targetTransform = PlayerTarget.transform;
        }
        else
        {
            int randomTargetIndex = Random.Range(0, _targets.Length);
            _targetTransform = _targets[randomTargetIndex].transform;
        }
    }

    private void StopMoving(Player player)
    {
        PlayerTarget = player;
        IsMoving = false;
    }

    private void StartMoving(Player player)
    {
        IsMoving = true;
        
        if (PlayerTarget != null && PlayerTarget == player)
        {
            PlayerTarget = null;
            SetTarget();
        }
    }
    
    private bool ReachedTarget()
    {
        if (_targetTransform == null) 
            return false;

        float distance = Mathf.Abs(transform.position.x - _targetTransform.position.x);
        return distance <= _maxDistance;
    }
}