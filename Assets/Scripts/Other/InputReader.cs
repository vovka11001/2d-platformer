using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
     private string _horizontalAxis = "Horizontal";
     private KeyCode _jumpKey = KeyCode.UpArrow;
     private KeyCode _attackKey = KeyCode.Space;
     private KeyCode _spellKey = KeyCode.LeftShift;
    
    public event Action Jumped;
    public event Action Attacked;
    public event Action Spelled;
    
    public float HorizontalInput { get; private set; }

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw(_horizontalAxis);
        
        if (Input.GetKeyDown(_jumpKey))
            Jumped?.Invoke();

        if (Input.GetKeyDown(_attackKey))
            Attacked?.Invoke();
        
        if (Input.GetKeyDown(_spellKey))
            Spelled?.Invoke();
    }
}