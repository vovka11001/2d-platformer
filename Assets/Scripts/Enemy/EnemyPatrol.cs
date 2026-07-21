using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float _maxDistance = 0.01f;
    [SerializeField] private Target[] _targets;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private PlayerDetector _playerDetector;

    const float Offset = 0.001f;
    
    public Transform TargetTransform { get; private set; }
    private Rigidbody2D _rigidbody2d;

    public Player PlayerTarget {get; private set;}
    public Vector2 LookDirection { get; private set; }

    private void OnEnable()
    {
        _playerDetector.PlayerChanged += UpdatePlayerTarget;
    }

    private void OnDisable()
    {
        _playerDetector.PlayerChanged -= UpdatePlayerTarget;
    }
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        SetTarget();
    }

    private void Update()
    {
        if (TargetTransform == null) 
            return;

        if (ReachedTarget())
            SetTarget();

        var offsetVector = (Vector2)TargetTransform.position - _rigidbody2d.position;
        Vector2 lookDirection = offsetVector.normalized;
        lookDirection.y = 0;
        LookDirection = lookDirection;
        
        if (offsetVector.sqrMagnitude > Offset)
            _rotator.FaceDirection(LookDirection);
    }
    
    public void RotateRight()
    {
        if (_rotator != null)
            _rotator.FaceRight();
    }

    public void RotateLeft()
    {
        if (_rotator != null)
            _rotator.FaceLeft();
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
            TargetTransform = PlayerTarget.transform;
        }
        else
        {
            int randomTargetIndex = Random.Range(0, _targets.Length);
            TargetTransform = _targets[randomTargetIndex].transform;
        }
    }
    
    private bool ReachedTarget()
    {
        if (TargetTransform == null) 
            return false;

        float distance = Mathf.Abs(transform.position.x - TargetTransform.position.x);
        return distance <= _maxDistance;
    }
}