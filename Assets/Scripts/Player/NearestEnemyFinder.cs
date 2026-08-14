using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearestEnemyFinder : MonoBehaviour
{
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _detectionRadius = 3f;
    [SerializeField] private int _maxDetectionCount = 16;

    private Collider2D[] _hitsBuffer;
    
    private void Awake()
    {
        _hitsBuffer = new Collider2D[_maxDetectionCount];
    }

    public Enemy FindNearest()
    {
        int hitCount = Physics2D.OverlapCircleNonAlloc(transform.position, _detectionRadius, _hitsBuffer, _enemyLayer);

        Enemy nearestEnemy = null;
        float nearestSqrDistance = float.MaxValue;

        for (int i = 0; i < hitCount; i++)
        {
            if (_hitsBuffer[i].TryGetComponent(out Enemy enemy) && enemy.IsDead == false)
            {
                float sqrDistance = ((Vector2)transform.position - (Vector2)enemy.transform.position).sqrMagnitude;

                if (sqrDistance < nearestSqrDistance)
                {
                    nearestSqrDistance = sqrDistance;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }
}
