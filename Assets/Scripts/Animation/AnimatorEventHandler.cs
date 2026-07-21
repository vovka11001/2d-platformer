using System;

using UnityEngine;

public class AnimatorEventHandler : MonoBehaviour
{
    public event Action Attacked;
    
    public void InvokeAttacked() => Attacked?.Invoke();
}
