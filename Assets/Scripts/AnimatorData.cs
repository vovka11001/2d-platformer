using UnityEngine;

public static class AnimatorData
{
    public static class Params
    {
        public static readonly int Grounded = Animator.StringToHash(nameof(Grounded));
        public static readonly int IsRunning = Animator.StringToHash(nameof(IsRunning));
    }
}
