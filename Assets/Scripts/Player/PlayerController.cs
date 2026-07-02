using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private EnemyDetector _enemyDetector;
    [SerializeField] private Player _player;

    private bool _canAttack = true;
    private List<Enemy> _currentEnemies;
    
    private Rigidbody2D _rigidbody2d;
    private Coroutine _attackCooldownCoroutine;

    private void OnEnable()
    {
        _enemyDetector.TriggerEntered += AddEnemy;
        _enemyDetector.TriggerExited += RemoveEnemy;
        _inputReader.Attacked += Attack;
        _inputReader.Jumped += Jump;
        _player.DecreasedHealth += SetAnimationHurt;
        _player.Death += SetAnimationDeath;
    }

    private void OnDisable()
    {
        _enemyDetector.TriggerEntered -= AddEnemy;
        _enemyDetector.TriggerExited -= RemoveEnemy;
        _inputReader.Attacked -= Attack;
        _inputReader.Jumped -= Jump;
        _player.DecreasedHealth -= SetAnimationHurt;
        _player.Death -= SetAnimationDeath;
    }

    private void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _currentEnemies = new List<Enemy>();
    }
    
    private void Update()
    {
        if (!_player.IsDead)
        {
            if (_inputReader.HorizontalInput > 0)
                _rotator.FaceLeft();
            else if (_inputReader.HorizontalInput < 0)
                _rotator.FaceRight();
            
            bool isGrounded = IsGrounded();
            bool shouldRun = Mathf.Abs(_inputReader.HorizontalInput) > 0 && isGrounded;
        
            _animator.SetBool(AnimatorData.Params.IsRunning, shouldRun);
            _animator.SetBool(AnimatorData.Params.Grounded, isGrounded);
        }
    }
    
    private void FixedUpdate()
    {
        if(!_player.IsDead)
            _rigidbody2d.velocity = new Vector2(_inputReader.HorizontalInput * _speed, _rigidbody2d.velocity.y);
    }

    private void Jump()
    {
        if(IsGrounded() && !_player.IsDead)
            _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, _jumpForce);
    }

    private void SetAnimationHurt()
    {
        _animator.SetBool(AnimatorData.Params.Hurt, true);
    }

    private void SetAnimationDeath()
    {
        _animator.SetBool(AnimatorData.Params.Death, true);
    }

    private void Attack()
    {
        if (!_player.IsDead)
        {
            if (!_canAttack)
                return;
        
            _canAttack = false;
            _animator.SetBool(AnimatorData.Params.Attack,true);
        
            if (_currentEnemies.Count > 0)
            {
                _currentEnemies.RemoveAll(enemy => enemy == null);
            
                foreach (var enemy in _currentEnemies)
                {
                    if (enemy != null)
                    {
                        enemy.DecreaseHealth(_player.Damage);
                    }
                }
            }
        
            if (_attackCooldownCoroutine != null)
                StopCoroutine(_attackCooldownCoroutine);
        
            _attackCooldownCoroutine = StartCoroutine(AttackCooldown());
        }
    }
    
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        _attackCooldownCoroutine = null;
    }
    
    private bool IsGrounded()
    {
        if (!_player.IsDead)
        {
            float distance = 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);
            return hit.collider != null;
        }
        
        return false;
    }

    private void AddEnemy(Enemy enemy)
    {
        _currentEnemies.Add(enemy);
    }
    
    private void RemoveEnemy(Enemy enemy)
    {
        _currentEnemies.Remove(enemy);
    }
}