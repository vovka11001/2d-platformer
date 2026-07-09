using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerMover : MonoBehaviour
{
    private Rigidbody2D _rigidbody2d;
    
    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Run(float speedDirection)
    {
        _rigidbody2d.velocity = new Vector2(speedDirection, _rigidbody2d.velocity.y);
    }
}
