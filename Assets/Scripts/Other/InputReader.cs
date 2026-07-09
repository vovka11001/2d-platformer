using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
     private string _horizontalAxis = "Horizontal";
     private KeyCode _jumpKey = KeyCode.UpArrow;
     private KeyCode _attackKey = KeyCode.Space;
    
    public event Action Jumped;
    public event Action Attacked;
    
    public float HorizontalInput { get; private set; }

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw(_horizontalAxis);
        
        if (Input.GetKeyDown(_jumpKey))
        {
            Jumped?.Invoke();
        }

        if (Input.GetKeyDown(_attackKey))
        {
            Attacked?.Invoke();
        }
    }
}