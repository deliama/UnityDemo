using Game.Gameplay.StateMachine.Character;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Combat
{
    public sealed class AttackRecoveryState : CharacterStateBase
    {
        private readonly float _duration;

        public AttackRecoveryState(CharacterStateContext context, float duration)
            : base(CharacterStateIds.AttackRecovery, context)
        {
            _duration = duration;
        }

        protected override void OnStateEnter()
        {
            BB.AttackWindowOpened = false;
            BB.CanMove = false;
            BB.CanRotate = true;
            BB.CanBeInterrupted = true;
            BB.DesiredMoveDirection = Vector3.zero;
            BB.MoveSpeed = 0f;
        }

        protected override void OnStateExit()
        {
            BB.CanMove = true;
            BB.CanRotate = true;
            BB.CanAcceptComboInput = false;
        }

        public bool IsFinished()
        {
            return BB.StateTime >= _duration;
        }
    }
}