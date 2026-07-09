using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Detector<T> : MonoBehaviour
{
    private readonly float _sizeX = 1f;
    private readonly float _sizeY = 1f;
    private readonly float _offsetY = 0.6f;
    
    private BoxCollider2D _collider;
    private T _detectedComponent;
    
    public event Action<T> TriggerEntered;
    public event Action<T> TriggerExited; 
    
    public bool IsOnTriggerEntered { get; private set; }

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _collider.isTrigger = true;
        _collider.size = new Vector2(_sizeX, _sizeY);
        _collider.offset = new Vector2(0, _offsetY);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out T component))
        {
            IsOnTriggerEntered = true;
            _detectedComponent = component;
            TriggerEntered?.Invoke(component);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out T component))
        {
            if (_detectedComponent != null && _detectedComponent.Equals(component))
            {
                _detectedComponent = default;
            }
            IsOnTriggerEntered = false;
            TriggerExited?.Invoke(component);
        }
    }
}