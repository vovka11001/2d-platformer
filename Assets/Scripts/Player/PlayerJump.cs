using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }
    
    public void Jump(float jumpForce)
    {
        _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, jumpForce);
    }
}
