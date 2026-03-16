using UnityEngine;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    public sealed class CharacterStateBlackboard
    {
        
        //输入
        public Vector2 MoveInput;
        public bool AttackPressed;
        public bool DashPressed;
        
        //移动
        public Vector3 DesiredMoveDirection;
        public float MoveSpeed;
        public float VerticalSpeed;

        //状态持续时间
        public float StateTime;
        public float LastAttackTime;

        //战斗
        public int ComboIndex;
        public bool AttackWindowOpened;
        public bool CanAcceptComboInput;
        public bool AttackPerformed;

        //角色状态
        public bool IsGrounded;
        public bool HitRequested;
        public bool DeadRequested;
        public bool IsHit;
        public bool IsDead;

        
        public bool CanMove = true;
        public bool CanRotate = true;
        public bool CanBeInterrupted = true;

    }
}