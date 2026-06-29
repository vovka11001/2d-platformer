using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    
    public event Action Jumped;
    public event Action Attacked;

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jumped?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attacked?.Invoke();
        }
    }
}
