using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private PlayerDetector _playerDetector;

    private float _speed = 4f;
    
    private Transform _targetTransform;
    private Rigidbody2D _rigidbody2d;

    public bool IsMoving { get; private set; } = true;
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        transform.position = _spawnPoint.position;
    }

    private void Update()
    {
        if (_playerDetector.IsOnTriggerEntered)
            IsMoving = false;
        else
            IsMoving = true;
    }

    private void FixedUpdate()
    {
        if (IsMoving && _targetTransform != null)
        {
            _rigidbody2d.position = Vector2.MoveTowards(_rigidbody2d.position, _targetTransform.position,
                _speed * Time.fixedDeltaTime);
        }
    }

    public void SetMovingFalse()
    {
        IsMoving = false;
    }

    public void SetTarget(Transform targetTransform)
    { 
        _targetTransform = targetTransform;
    }
}