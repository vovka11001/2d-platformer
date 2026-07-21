using System;
using UnityEngine;

public class PlayerDetector : Detector<Player>
{
    [SerializeField] private EnemyPatrol _enemyPatrol;
    [SerializeField] private LayerMask _playerLayer;

    private Player _currentPlayer;
    
    public event Action<Player> PlayerChanged;
    
    private void Update()
    {
        SearchPlayer();
    }
    
    private void SearchPlayer()
    {
        float distance = 5f;
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y + 1f);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, _enemyPatrol.LookDirection, distance, _playerLayer);
        
        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out Player player))
            {
                if (_currentPlayer == null)
                {
                    PlayerChanged?.Invoke(player);
                }

                _currentPlayer = player;
            }
            else
            {
                ReleaseTargetPlayer();
            }
        }
        else
        {
            ReleaseTargetPlayer();
        }
    }

    private void ReleaseTargetPlayer()
    {
        if (_currentPlayer != null)
        {
            PlayerChanged?.Invoke(null);
        }

        _currentPlayer = null;
    }
}