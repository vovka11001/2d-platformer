using System.Collections;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    
    private readonly float _checkRadius = 0.2f;
    private static readonly float _groundCheckCoolDown = 0.1f;
    
    private readonly WaitForSeconds _waitForSeconds =  new WaitForSeconds(_groundCheckCoolDown);
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