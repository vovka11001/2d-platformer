using UnityEngine;

public class AnimatorControler : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void RunningAnimation()
    {
        foreach (AnimationClip clip in _animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "Run")
            {
                clip.wrapMode = WrapMode.Loop;
                _animator.Play(clip.name);
            }
        }
    }
}
