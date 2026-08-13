using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SpellChargeBar : MonoBehaviour
{
    [SerializeField] private PlayerVampirism vampirism;

    private float _fillSpeed = 20f;
    private float _valueThreshold = 0.001f;

    private Slider _slider;
    private float _targetValue;
    private Coroutine _smoothFillCoroutine;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        vampirism.DurationChanged += OnDurationChanged;
        vampirism.CooldownChanged += OnCooldownChanged;
    }

    private void OnDisable()
    {
        vampirism.DurationChanged -= OnDurationChanged;
        vampirism.CooldownChanged -= OnCooldownChanged;

        if (_smoothFillCoroutine != null)
            StopCoroutine(_smoothFillCoroutine);
    }

    private void Start()
    {
        _slider.value = 1f;
        _targetValue = 1f;
    }

    private void OnDurationChanged(float timeRemaining, float totalDuration)
    {
        SetTarget(timeRemaining / totalDuration);
    }

    private void OnCooldownChanged(float elapsedTime, float totalCooldown)
    {
        SetTarget(elapsedTime / totalCooldown);
    }

    private void SetTarget(float targetValue)
    {
        _targetValue = targetValue;

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
