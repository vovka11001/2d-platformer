using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SmoothHealthBar : SmoothBar
{
    private IDamageable _damageable;

    protected override void Awake()
    {
        base.Awake();
        _damageable = GetComponentInParent<IDamageable>();
    }

    private void OnEnable()
    {
        if (_damageable != null)
            _damageable.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        if (_damageable != null)
            _damageable.HealthChanged -= OnHealthChanged;

        StopSmoothing();
    }

    private void Start()
    {
        if (_damageable == null)
            return;

        Slider.maxValue = _damageable.MaxHealth;
        SetValueInstant(_damageable.Health);
    }

    private void OnHealthChanged(int current, int max)
    {
        Slider.maxValue = max;
        SetTarget(current);
    }
}