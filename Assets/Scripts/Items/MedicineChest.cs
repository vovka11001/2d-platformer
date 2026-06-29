using UnityEngine;

[RequireComponent (typeof(CircleCollider2D))]
public class MedicineChest : MonoBehaviour
{
    private CircleCollider2D _collider2D;
    public int IncreaseAmount { get; private set; }

    private void Awake()
    {
        _collider2D = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        _collider2D.isTrigger = true;
        transform.localScale = new Vector2(0.2f, 0.2f);
        IncreaseAmount = 20;
    }
}
