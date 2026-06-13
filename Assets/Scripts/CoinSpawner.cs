using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private CoinDetector _coinDetector;

    private void OnEnable()
    {
        _coinDetector.TriggerEntered += DeleteCoin;
    }

    private void OnDisable()
    {
        _coinDetector.TriggerEntered -= DeleteCoin;
    }

    private void Start()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
            return;

        int coinsCount = _spawnPoints.Length;

        for (int i = 0; i < coinsCount; i++)
        {
            Coin newCoin = Instantiate(_coinPrefab, _spawnPoints[i].position, Quaternion.identity);
        }
    }

    private void DeleteCoin(Coin coin)
    {
        Destroy(coin.gameObject);
    }
}