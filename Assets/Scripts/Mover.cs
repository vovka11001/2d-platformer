using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private float _maxDistance = 0.001f;
    private Vector2 _targetPosition;
    
    private void Start()
    {
        _maxDistance *= _maxDistance;
    }
    
    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
    }

    public void SetTarget(Vector2 targetPosition)
    {
        _targetPosition = targetPosition;
    }
    
    public bool ReachedTarget()
    {
        Vector2 offset = (Vector2)transform.position - _targetPosition;
        return offset.sqrMagnitude <= _maxDistance;
    }
}
