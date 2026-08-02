using System;
using UnityEngine;

public class PlayerDetector : Detector<Player>
{
    [SerializeField] private LayerMask _playerLayer;

    private Player _currentPlayer;
    private Vector2 _direction;
    
    public event Action<Player> PlayerChanged;
    
    private void Update()
    {
        SearchPlayer();
    }
    
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }
    
    private void SearchPlayer()
    {
        float distance = 5f;
        Vector2 raycastPosition = new Vector2(transform.position.x, transform.position.y + 1f);
        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, _direction, distance, _playerLayer);
        
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