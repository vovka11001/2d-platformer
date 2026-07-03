using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        SetGrounded(IsGrounded());
    }

    public void SetAnimationRun(bool canRun)
    {
        if(IsGrounded())
            _animator.SetBool(AnimatorData.Params.IsRunning, canRun);
    }

    public void SetAnimationHurt()
    {
        _animator.SetBool(AnimatorData.Params.Hurt, true);
    }
    
    public void SetAnimationAttacking()
    {
        _animator.SetBool(AnimatorData.Params.Attack, true);
    }
    
    public void SetAnimationDie()
    {
        _animator.SetBool(AnimatorData.Params.Death, true);
    }

    public void SetGrounded(bool grounded)
    {
        _animator.SetBool(AnimatorData.Params.Grounded, IsGrounded());
    }

    private bool IsGrounded()
    {
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);

        return hit.collider != null;
    }
}