using System;
using UnityEngine;

public class PlayerDetector : Detector<Player>
{
    [SerializeField] private EnemyMover _enemyMover;
    
    private Player _currentPlayer;
    
    public event Action<Player> PlayerChanged;
    
    private void Update()
    {
        IsPlayerFound();
    }
    
    private void IsPlayerFound()
    {
        float distance = 5f;
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y + 1f);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, _enemyMover.LookDirection, distance, LayerMask.GetMask("Player"));

        Debug.DrawRay(raycastPosition, _enemyMover.LookDirection * distance, Color.white);

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

        void ReleaseTargetPlayer()
        {
            if (_currentPlayer != null)
            {
                PlayerChanged?.Invoke(null);
            }

            _currentPlayer = null;
        }
    }
}