using Game.Gameplay.StateMachine.Character;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    /// <summary>
    /// 终止状态：进入后角色不可再恢复为普通行为态。
    /// </summary>
    public sealed class DeadState : CharacterStateBase
    {
        public DeadState(CharacterStateContext context)
            : base(CharacterStateIds.Dead, context)
        {
        }

        protected override void OnStateEnter()
        {
            BB.DeadRequested = false;
            BB.HitRequested = false;
            BB.AttackPressed = false;

            BB.IsHit = false;
            BB.IsDead = true;
            BB.CanMove = false;
            BB.CanRotate = false;
            BB.CanBeInterrupted = false;
            BB.AttackWindowOpened = false;
            BB.AttackPerformed = false;
            BB.DesiredMoveDirection = Vector3.zero;
            BB.MoveSpeed = 0f;
        }
    }
}

