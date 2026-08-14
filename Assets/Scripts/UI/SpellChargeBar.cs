using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SpellChargeBar : SmoothBar
{
    private float _value = 1f;
    
    private void OnDisable()
    {
        StopSmoothing();
    }

    public void SetDurationProgress(float timeRemaining, float totalDuration)
    {
        SetTarget(timeRemaining / totalDuration);
    }

    public void SetCooldownProgress(float elapsedTime, float totalCooldown)
    {
        SetTarget(elapsedTime / totalCooldown);
    }

    public void ResetToFull()
    {
        SetValueInstant(_value);
    }
}