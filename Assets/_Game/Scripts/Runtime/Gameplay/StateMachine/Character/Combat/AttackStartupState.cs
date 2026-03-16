using Game.Gameplay.StateMachine.Character;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Combat
{
    public sealed class AttackStartupState : CharacterStateBase
    {
        private readonly float _duration;

        public AttackStartupState(CharacterStateContext context, float duration)
            : base(CharacterStateIds.AttackStartup, context)
        {
            _duration = duration;
        }

        protected override void OnStateEnter()
        {
            // 消费攻击请求，避免同一次输入重复触发进入。
            BB.AttackPressed = false;
            BB.AttackWindowOpened = false;
            BB.AttackPerformed = false;

            BB.CanMove = false;
            BB.CanRotate = false;
            BB.CanBeInterrupted = true;
            BB.DesiredMoveDirection = Vector3.zero;
            BB.MoveSpeed = 0f;
            
            Ctx?.Animator.SetMoveSpeed(0);
            Ctx?.Animator.PlayAttack();
        }

        public bool IsFinished()
        {
            return BB.StateTime >= _duration;
        }
    }
}