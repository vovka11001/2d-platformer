using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints; 
    [SerializeField] private Item _prefab;
    [SerializeField] private ItemDetector _itemDetector;
    
    private void Start()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
            return;

        int count = _spawnPoints.Length;

        for (int i = 0; i < count; i++)
        {
             Instantiate(_prefab, _spawnPoints[i].position, Quaternion.identity);
        }
    }
}