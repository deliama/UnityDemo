using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character.Combat
{
    public sealed class AttackActiveState : CharacterStateBase
    {
        private readonly float _duration;

        public AttackActiveState(CharacterStateContext context, float duration)
            : base(CharacterStateIds.AttackActive, context)
        {
            _duration = duration;
        }

        protected override void OnStateEnter()
        {
            BB.AttackWindowOpened = true;
            BB.CanMove = false;
            BB.CanRotate = false;
            BB.CanBeInterrupted = true;
        }

        protected override void OnStateTick(float deltaTime)
        {
            // 最小原型：Active 期间仅允许一次“生效”标记，后续可接真正命中逻辑。
            if (!BB.AttackPerformed)
            {
                BB.AttackPerformed = true;
            }
        }

        protected override void OnStateExit()
        {
            BB.AttackWindowOpened = false;
        }

        public bool IsFinished()
        {
            return BB.StateTime >= _duration;
        }
    }
}