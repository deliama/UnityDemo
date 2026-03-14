using UnityEngine;
using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Grounded
{
    public sealed class MoveState : CharacterStateBase
    {
        public MoveState(CharacterStateContext context)
            : base(CharacterStateIds.Move, context)
        {
            
        }

        protected override void OnStateEnter()
        {
            
        }

        protected override void OnStateTick(float deltaTime)
        {
            Vector2 input = BB.MoveInput;
            Vector3 moveDirection = new Vector3(input.x, 0f, input.y);

            if (moveDirection.sqrMagnitude > 1f)
            {
                moveDirection.Normalize();
            }

            BB.DesiredMoveDirection = moveDirection;
            //BB.MoveSpeed = input.magnitude;
            BB.MoveSpeed = Mathf.Clamp01(input.magnitude);
        }

        protected override void OnStateExit()
        {
            BB.MoveSpeed = 0f;
            BB.DesiredMoveDirection = Vector3.zero;
        }
    }
}