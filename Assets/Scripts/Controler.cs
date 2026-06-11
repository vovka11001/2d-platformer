using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Controler : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Animator _animator;
    
    private float _horizontalInput;
    private Rigidbody2D _rigidbody2d;
    private Collider2D _collider;

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }
    
    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (_horizontalInput > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        
        else if (_horizontalInput < 0)
            transform.localScale = new Vector3(1, 1, 1);
        
        bool isGrounded = IsGrounded();
        bool shouldRun = Mathf.Abs(_horizontalInput) > 0 && isGrounded;
        
        _animator.SetBool("IsRunning", shouldRun);
        _animator.SetBool("IsJumping", !isGrounded);
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _jumpForce);

        }
    }
    
    private void FixedUpdate()
    {
        _rigidbody2d.velocity = new Vector2(_horizontalInput * _speed, _rigidbody2d.velocity.y);
    }
    
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_rigidbody2d.transform.position, Vector2.down, _collider.bounds.extents.y);

        Debug.DrawRay(_rigidbody2d.transform.position,Vector2.down * _collider.bounds.extents.y);
        return hit.collider != null;
    }
}