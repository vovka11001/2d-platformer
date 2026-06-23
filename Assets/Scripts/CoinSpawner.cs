using UnityEngine;

public class CoinSpawner : Spawner<Coin>
{
    [SerializeField] private CoinDetector _detector;
    
    private void OnEnable()
    {
        _detector.TriggerEntered += DestroyCoin;
    }

    private void OnDisable()
    {
        _detector.TriggerEntered -= DestroyCoin;
    }

    private void DestroyCoin(Coin coin)
    {
        Destroy(coin.gameObject);
    }
}
