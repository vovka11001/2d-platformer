using UnityEngine;

public class Controler : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private Player _player;
    [SerializeField] private Animator _animator;
    
    private float _horizontalInput;
    private bool _isJumping;
    
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
        
        if (shouldRun && !_isJumping)
            _animator.Play("Run");
        
        else if (isGrounded && !shouldRun && !_isJumping)
            _animator.Play("Idle");
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, _jumpForce);
            _isJumping = true;
            _animator.SetBool("IsJumping", true);
            _animator.Play("Jump");
        }
        
        if (isGrounded)
        {
            _isJumping = false;
            _animator.SetBool("IsJumping", false);
        }
    }
    
    private void FixedUpdate()
    {
        _player.Rigidbody.velocity = new Vector2(_horizontalInput * _speed, _player.Rigidbody.velocity.y);
    }
    
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_player.transform.position, Vector2.down, _player.Collider.bounds.extents.y);
        
        return hit.collider != null;
    }
}