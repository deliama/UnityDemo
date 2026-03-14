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
        }

        protected override void OnStateTick(float deltaTime)
        {
            base.OnStateTick(deltaTime);
        }

        protected override void OnStateExit()
        {
            base.OnStateExit();
        }
    }
}