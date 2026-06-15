using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Animator _animator;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private Rigidbody2D _rigidbody2d;

    private void OnEnable()
    {
        _inputReader.Jumped += Jump;
    }

    private void OnDisable()
    {
        _inputReader.Jumped -= Jump;
    }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        if (_inputReader.HorizontalInput > 0)
            _spriteRenderer.flipX = true;
        else if (_inputReader.HorizontalInput < 0)
            _spriteRenderer.flipX = false;
        
        bool isGrounded = IsGrounded();
        bool shouldRun = Mathf.Abs(_inputReader.HorizontalInput) > 0 && isGrounded;
        
        _animator.SetBool(AnimatorData.Params.IsRunning, shouldRun);
        _animator.SetBool(AnimatorData.Params.Grounded, isGrounded);
    }
    
    private void FixedUpdate()
    {
        _rigidbody2d.velocity = new Vector2(_inputReader.HorizontalInput * _speed, _rigidbody2d.velocity.y);
    }

    private void Jump()
    {
        if(IsGrounded())
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _jumpForce);
    }
    
    private bool IsGrounded()
    {
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(_rigidbody2d.transform.position, Vector2.down, distance);

        return hit.collider != null;
    }
}