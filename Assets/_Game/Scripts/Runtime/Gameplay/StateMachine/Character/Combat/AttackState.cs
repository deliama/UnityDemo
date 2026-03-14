using UnityEngine;
using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Combat
{
    public sealed class AttackState : CharacterStateBase
    {
        readonly float _duration;

        public AttackState(CharacterStateContext context, float duration)
            : base(CharacterStateIds.Attack, context)
        {
            _duration = duration;
        }
        
        protected override void OnStateEnter()
        {
            BB.CanMove = false;
            BB.CanRotate = true;
            BB.CanBeInterrupted = true;

            BB.AttackPressed = false;
            BB.AttackPerformed = false;
            BB.AttackWindowOpened = false;

            BB.DesiredMoveDirection = Vector3.zero;
            BB.MoveSpeed = 0f;
        }

        protected override void OnStateTick(float deltaTime)
        {
        }

        protected override void OnStateExit()
        {
            BB.CanMove = true;
            BB.CanRotate = true;

            BB.AttackPressed = false;
            BB.AttackWindowOpened = false;
            BB.AttackPerformed = false;
        }

        public bool IsFinished()
        {
            return BB.StateTime >= _duration;
        }
    }
}