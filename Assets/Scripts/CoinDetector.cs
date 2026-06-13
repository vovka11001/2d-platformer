using UnityEngine;
using System;

public class CoinDetector : MonoBehaviour
{
    public event Action<Coin> TriggerEntered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Coin coin))
            TriggerEntered?.Invoke(coin);
    }
}