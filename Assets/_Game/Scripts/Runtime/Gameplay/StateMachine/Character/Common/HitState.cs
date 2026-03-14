using Game.Gameplay.StateMachine.Character;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    public sealed class HitState : CharacterStateBase
    {
        private readonly float _duration;

        public HitState(CharacterStateContext context, float duration)
            : base(CharacterStateIds.Hit, context)
        {
            _duration = duration;
        }

        protected override void OnStateEnter()
        {
            BB.HitRequested = false;
            BB.IsHit = true;
            BB.CanMove = false;
            BB.CanRotate = false;
            BB.DesiredMoveDirection = Vector3.zero;
            BB.MoveSpeed = 0f;
        }

        protected override void OnStateTick(float deltaTime)
        {
        }

        protected override void OnStateExit()
        {
            BB.IsHit = false;
            BB.CanMove = true;
            BB.CanRotate = true;
            //Debug.Log("Exit Hit");
        }

        public bool IsFinished()
        {
            return BB.StateTime >= _duration;
        }
    }
}