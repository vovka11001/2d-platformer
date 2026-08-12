using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SmoothHealthBar : MonoBehaviour
{
    private float _targetValue;
    private float _fillSpeed = 50f;
    private float _valueThreshold = 0.01f;

    private Slider _slider;
    private IDamageable _damageable;
    private Coroutine _smoothFillCoroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _damageable = GetComponentInParent<IDamageable>();
    }

    private void OnEnable()
    {
        if (_damageable != null)
            _damageable.HealthChanged += SetTarget;
    }

    private void OnDisable()
    {
        if (_damageable != null)
            _damageable.HealthChanged -= SetTarget;

        if (_smoothFillCoroutine != null)
            StopCoroutine(_smoothFillCoroutine);
    }

    private void Start()
    {
        if (_damageable == null)
            return;

        _slider.maxValue = _damageable.MaxHealth;
        _slider.value = _damageable.Health;
        _targetValue = _damageable.Health;
    }

    private void SetTarget(int current, int max)
    {
        _slider.maxValue = max;
        _targetValue = current;

        if (_smoothFillCoroutine != null)
            StopCoroutine(_smoothFillCoroutine);

        _smoothFillCoroutine = StartCoroutine(SmoothFill());
    }

    private IEnumerator SmoothFill()
    {
        while (Mathf.Abs(_slider.value - _targetValue) > _valueThreshold)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _targetValue, _fillSpeed * Time.deltaTime);
            yield return null;
        }

        _slider.value = _targetValue;
        _smoothFillCoroutine = null;
    }
}