using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private Target[] _targets;
    [SerializeField] private Enemy _enemyPrefab;
    
    private int _enemyCount = 2;
    
    private Mover[] _currentMovers;  
    
    private void Start()
    {
        _currentMovers = new Mover[_enemyCount];
        
        for (int i = 0; i < _enemyCount; i++)
        {
            int randomSpawnIndex = Random.Range(0, _spawnPoint.Length);
            
            Enemy newEnemy = Instantiate(_enemyPrefab, _spawnPoint[randomSpawnIndex].position, Quaternion.identity);
            
            if (newEnemy.TryGetComponent(out Mover mover))
            {
                _currentMovers[i] = mover;
                
                int randomTargetIndex = Random.Range(0, _targets.Length);
                Vector2 targetPosition = _targets[randomTargetIndex].transform.position;
                mover.SetTarget(targetPosition);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < _currentMovers.Length; i++)
        {
            if (_currentMovers[i] != null && _currentMovers[i].ReachedTarget())
            {
                int randomTargetIndex = Random.Range(0, _targets.Length);
                Vector2 newTargetPosition = _targets[randomTargetIndex].transform.position;
                _currentMovers[i].SetTarget(newTargetPosition);
            }
        }
    }
}
