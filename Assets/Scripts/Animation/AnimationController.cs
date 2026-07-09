using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetAnimationRun(bool canRun)
    {
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
        _animator.SetBool(AnimatorData.Params.Grounded, grounded);
    }
}