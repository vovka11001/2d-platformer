using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxDistance = 0.01f;

    private Vector2 _targetPosition;
    private Rigidbody2D _rigidbody2d;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        _rigidbody2d.position = Vector2.MoveTowards(_rigidbody2d.position, _targetPosition, _speed * Time.fixedDeltaTime);
    }

    public void SetTarget(Vector2 targetPosition)
    {
        _targetPosition = targetPosition;
        Debug.Log(targetPosition);
    }
    
    public bool ReachedTarget()
    {
        float distance = Mathf.Abs(transform.position.x - _targetPosition.x);
        return distance <= _maxDistance;
    }
}
