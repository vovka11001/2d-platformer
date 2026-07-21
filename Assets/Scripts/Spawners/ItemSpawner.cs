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

        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            SpawnItem(i);
        }
    }

    private void SpawnItem(int index)
    {
        Item newItem = Instantiate(_prefab, _spawnPoints[index].position, Quaternion.identity);
        newItem.Collected += DestroyItem;
    }
    
    private void DestroyItem(Item item)
    {
        item.Collected -= DestroyItem;
        Destroy(item.gameObject);
    }
}