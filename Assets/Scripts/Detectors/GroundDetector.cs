using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    
    private float _checkRadius = 0.2f;
    private static float _groundCheckCoolDown = 0.1f;
    private readonly float _sizeX = 1f;
    private readonly float _sizeY = 1f;
    private readonly float _offsetY = 0.6f;
    
    private WaitForSeconds _waitForSeconds =  new WaitForSeconds(_groundCheckCoolDown);
    private BoxCollider2D _collider;
    private Coroutine _groundCheckCoroutine;
    
    private void OnEnable()
    {
        if (_groundCheckCoroutine != null)
            StopCoroutine(_groundCheckCoroutine);
        
        _groundCheckCoroutine = StartCoroutine(GroundedCheck());
    }
    
    private void OnDisable()
    {
        if (_groundCheckCoroutine != null)
        {
            StopCoroutine(_groundCheckCoroutine);
            _groundCheckCoroutine = null;
        }
    }
    
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
    
    public bool IsGrounded()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _checkRadius, _groundLayer);
        return hit != null;
    }

    private IEnumerator GroundedCheck()
    {
        yield return _waitForSeconds;
        IsGrounded();
    }
}