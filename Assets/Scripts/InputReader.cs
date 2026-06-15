using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    
    public event Action Jumped;

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jumped?.Invoke();
        }
    }
}
