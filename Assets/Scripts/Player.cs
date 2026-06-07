using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    public Collider2D Collider {get; private set;}
    public Rigidbody2D Rigidbody {get; private set;}
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<Collider2D>();
    }
    private void Start()
    {
        transform.position = _spawnPoint.transform.position;
    }
}
