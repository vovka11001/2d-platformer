using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class GroundDetector : MonoBehaviour
{
    private readonly float _sizeX = 1f;
    private readonly float _sizeY = 1f;
    private readonly float _offsetY = 0.6f;
    
    private BoxCollider2D _collider;
    
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _collider.isTrigger = true;
        _collider.size = new Vector2(_sizeX, _sizeY);
        _collider.offset = new Vector2(0, _offsetY);
    }
    
    private void Update()
    {
        IsGrounded();
    }
    
    public bool IsGrounded()
    {
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);
        return hit.collider != null;
    }
}