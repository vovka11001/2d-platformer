using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float _pressedScaleMultiplier = 0.9f;
    private float _normalScaleMultiplier = 1f;
    private float _bounceDuration = 0.1f;

    private Vector3 _originalScale;
    private Coroutine _bounceCoroutine;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartBounce(_pressedScaleMultiplier);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartBounce(_normalScaleMultiplier);
    }

    private void StartBounce(float targetScaleMultiplier)
    {
        if (_bounceCoroutine != null)
            StopCoroutine(_bounceCoroutine);

        _bounceCoroutine = StartCoroutine(ScaleTo(_originalScale * targetScaleMultiplier));
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < _bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            float animationProgress = elapsedTime / _bounceDuration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, animationProgress);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}