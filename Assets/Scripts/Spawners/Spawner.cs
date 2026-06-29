using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : Component
{
    [SerializeField] private Transform[] _spawnPoints; 
    [SerializeField] private T _prefab;
    
    private void Start()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
            return;

        int count = _spawnPoints.Length;

        for (int i = 0; i < count; i++)
        {
            var newObject = Instantiate(_prefab, _spawnPoints[i].position, Quaternion.identity);
        }
    }
}