using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxDistance = 0.01f;
    [SerializeField] private Target[] _targets;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private Vector2 _targetPosition;
    private Vector2 _previousPosition;
    private Rigidbody2D _rigidbody2d;

    public event UnityAction Runned;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        transform.position = _spawnPoint.position;
        _previousPosition = _rigidbody2d.position;
        
        SetTarget();
    }
    
    private void FixedUpdate()
    {
        _rigidbody2d.position = Vector2.MoveTowards(_rigidbody2d.position, _targetPosition, _speed * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (ReachedTarget())
        {
            SetTarget();
        }

        Vector2 direction = _rigidbody2d.position - _previousPosition;
        float offset = 0.001f;

        if (direction.sqrMagnitude > offset)
        {
            Runned?.Invoke();
            if (direction.x > 0)
                _spriteRenderer.flipX = true;
            else if (direction.x < 0)
                _spriteRenderer.flipX = false;
        }

        _previousPosition = _rigidbody2d.position;
    }

    public void SetTarget()
    {
        int randomTargetIndex = Random.Range(0, _targets.Length);
        Vector2 targetPosition = _targets[randomTargetIndex].transform.position;
        _targetPosition = targetPosition;
    }
    
    public bool ReachedTarget()
    {
        float distance = Mathf.Abs(transform.position.x - _targetPosition.x);
        return distance <= _maxDistance;
    }
}