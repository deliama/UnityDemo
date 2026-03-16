using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Character
{
    public sealed class CharacterAnimatorController
    {
        private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private static readonly int HitHash = Animator.StringToHash("Hit");

        private readonly Animator _animator;

        public CharacterAnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public void SetMoveSpeed(float speed)
        {
            if (_animator == null) return;
            _animator.SetFloat(MoveSpeedHash, speed);
        }

        public void PlayAttack()
        {
            if (_animator == null) return;
            _animator.ResetTrigger(HitHash);
            _animator.SetTrigger(AttackHash);
        }

        public void PlayHit()
        {
            if (_animator == null) return;
            _animator.ResetTrigger(AttackHash);
            _animator.SetTrigger(HitHash);
        }

        public Animator RawAnimator => _animator;
    }
}