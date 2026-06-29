using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Enemy _enemy;

    private void OnEnable()
    {
        _enemy.Hurt += SetAnimationHurt;
        _mover.Runned += SetAnimationRun;
        _mover.StopedMoving += SetAnimationIdle;
        _mover.Attacking += SetAnimationAttacking;
        _enemy.Death += SetAnimationDie;
    }

    private void OnDisable()
    {
        _enemy.Hurt -= SetAnimationHurt;
        _mover.Runned -= SetAnimationRun;
        _mover.StopedMoving -= SetAnimationIdle;
        _mover.Attacking -= SetAnimationAttacking;
        _enemy.Death += SetAnimationDie;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(AnimatorData.Params.Grounded, IsGrounded());
    }

    private void SetAnimationRun()
    {
        if(IsGrounded())
            _animator.SetBool(AnimatorData.Params.IsRunning, true);
    }

    private void SetAnimationHurt()
    {
        _animator.SetBool(AnimatorData.Params.Hurt, true);
    }
    
    private void SetAnimationIdle()
    {
        _animator.SetBool(AnimatorData.Params.IsRunning, false);
    }
    private void SetAnimationAttacking()
    {
        _animator.SetBool(AnimatorData.Params.Attack, true);
    }
    
    private void SetAnimationDie()
    {
        _animator.SetBool(AnimatorData.Params.Death, true);
    }

    private bool IsGrounded()
    {
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);

        return hit.collider != null;
    }
}