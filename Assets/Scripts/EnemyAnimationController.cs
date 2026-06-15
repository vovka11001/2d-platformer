using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private EnemyMover _mover;

    private void OnEnable()
    {
        _mover.Runned += SetAnimation;
    }

    private void OnDisable()
    {
        _mover.Runned -= SetAnimation;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetBool(AnimatorData.Params.Grounded, IsGrounded());
    }

    private void SetAnimation()
    {
        if(IsGrounded())
            _animator.SetBool(AnimatorData.Params.IsRunning, true);
    }

    private bool IsGrounded()
    {
        float distance = 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);

        return hit.collider != null;
    }
}