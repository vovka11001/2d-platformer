using UnityEngine;

[RequireComponent (typeof(CircleCollider2D))]

public class Item : MonoBehaviour
{
    private CircleCollider2D _collider2D;
    private void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _collider2D.isTrigger = true;
        transform.localScale = new Vector2(0.2f, 0.2f);
    }

    public void Collect()
    {
        Destroy(gameObject);
    }
}