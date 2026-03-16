using UnityEngine;
using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Grounded
{
    public sealed class IdleState : CharacterStateBase
    {
        public IdleState(CharacterStateContext context)
            : base(CharacterStateIds.Idle, context)
        {
            
        }

        protected override void OnStateEnter()
        {
            base.OnStateEnter();
            BB.MoveSpeed = 0f;
            BB.DesiredMoveDirection = Vector3.zero;
            Ctx?.Animator.SetMoveSpeed(0);
        }

        protected override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
            Ctx.Animator.SetMoveSpeed(0);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}