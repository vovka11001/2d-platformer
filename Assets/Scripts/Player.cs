using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;
    
    private void Start()
    {
        transform.position = _spawnPoint.transform.position;
    }
}
