using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SmoothBar : MonoBehaviour
{
    [SerializeField] private float _fillSpeed = 20f;
    [SerializeField] private float _valueThreshold = 0.01f;

    private float _targetValue;
    private Coroutine _smoothFillCoroutine;
    
    protected Slider Slider { get; private set; }

    protected virtual void Awake()
    {
        Slider = GetComponent<Slider>();
    }

    protected void SetValueInstant(float value)
    {
        Slider.value = value;
        _targetValue = value;
    }

    protected void SetTarget(float targetValue)
    {
        _targetValue = targetValue;

        if (_smoothFillCoroutine != null)
            StopCoroutine(_smoothFillCoroutine);

        _smoothFillCoroutine = StartCoroutine(SmoothFill());
    }

    protected void StopSmoothing()
    {
        if (_smoothFillCoroutine != null)
        {
            StopCoroutine(_smoothFillCoroutine);
            _smoothFillCoroutine = null;
        }
    }

    private IEnumerator SmoothFill()
    {
        while (Mathf.Abs(Slider.value - _targetValue) > _valueThreshold)
        {
            Slider.value = Mathf.MoveTowards(Slider.value, _targetValue, _fillSpeed * Time.deltaTime);
            yield return null;
        }

        Slider.value = _targetValue;
        _smoothFillCoroutine = null;
    }
}