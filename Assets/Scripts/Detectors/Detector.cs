using System;
using System.Collections;
using UnityEngine;

public class Detector<T> : MonoBehaviour
{ 
    [SerializeField] protected LayerMask _detectableLayer;

    private readonly float _sizeX = 1f;
    private readonly float _sizeY = 1f;
    private readonly float _offsetY = 0.6f;
    private readonly float _overlapAngle = 0f;
    private readonly float _checkInterval = 0.1f;

    private WaitForSeconds _waitForCheckInterval;
    private Coroutine _overlapCheckCoroutine;
    private T _detectedComponent;

    public event Action<T> TriggerEntered;
    public event Action<T> TriggerExited;

    public bool IsOnTriggerEntered { get; private set; }

    private void Awake()
    {
        _waitForCheckInterval = new WaitForSeconds(_checkInterval);
    }

    private void OnEnable()
    {
        if (_overlapCheckCoroutine != null)
            StopCoroutine(_overlapCheckCoroutine);

        _overlapCheckCoroutine = StartCoroutine(OverlapCheckLoop());
    }

    private void OnDisable()
    {
        if (_overlapCheckCoroutine != null)
        {
            StopCoroutine(_overlapCheckCoroutine);
            _overlapCheckCoroutine = null;
        }
    }

    private IEnumerator OverlapCheckLoop()
    {
        while (true)
        {
            CheckOverlap();
            yield return _waitForCheckInterval;
        }
    }

    private void CheckOverlap()
    {
        Vector2 overlapCenter = (Vector2)transform.position + new Vector2(0, _offsetY);
        Vector2 overlapSize = new Vector2(_sizeX, _sizeY);
        Collider2D hitCollider = Physics2D.OverlapBox(overlapCenter, overlapSize, _overlapAngle, _detectableLayer);

        if (hitCollider != null && hitCollider.TryGetComponent(out T foundComponent))
        {
            if (IsOnTriggerEntered == false)
            {
                IsOnTriggerEntered = true;
                _detectedComponent = foundComponent;
                TriggerEntered?.Invoke(foundComponent);
            }
        }
        else if (IsOnTriggerEntered)
        {
            IsOnTriggerEntered = false;
            TriggerExited?.Invoke(_detectedComponent);
            _detectedComponent = default;
        }
    }
}