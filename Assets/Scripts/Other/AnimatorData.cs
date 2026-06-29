using UnityEngine;

public static class AnimatorData
{
    public static class Params
    {
        public static readonly int Grounded = Animator.StringToHash(nameof(Grounded));
        public static readonly int IsRunning = Animator.StringToHash(nameof(IsRunning));
        public static readonly int Attack = Animator.StringToHash(nameof(Attack));
        public static readonly int Hurt = Animator.StringToHash(nameof(Hurt));
        public static readonly int Death = Animator.StringToHash(nameof(Death));
    }
}
