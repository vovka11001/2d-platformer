using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyMover _mover;
    [SerializeField] private Enemy _enemy;

    private void OnEnable()
    {
        _enemy.Hurt += SetAnimationHurt;
        _enemy.Attacking += SetAnimationAttacking;
        _enemy.Death += SetAnimationDie;
    }

    private void OnDisable()
    {
        _enemy.Hurt -= SetAnimationHurt;
        _enemy.Attacking -= SetAnimationAttacking;
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

    public void SetAnimationRun()
    {
        if(IsGrounded())
            _animator.SetBool(AnimatorData.Params.IsRunning, true);
    }

    public void SetAnimationHurt()
    {
        _animator.SetBool(AnimatorData.Params.Hurt, true);
    }
    
    public void SetAnimationIdle()
    {
        _animator.SetBool(AnimatorData.Params.IsRunning, false);
    }
    public void SetAnimationAttacking()
    {
        _animator.SetBool(AnimatorData.Params.Attack, true);
    }
    
    public void SetAnimationDie()
    {
        _animator.SetBool(AnimatorData.Params.Death, true);
    }

    public bool IsGrounded()
    {
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);

        return hit.collider != null;
    }
}