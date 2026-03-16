using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    public abstract class CharacterStateBase : IHfsmState
    {
        protected CharacterStateContext Ctx { get; }
        protected CharacterStateBlackboard BB => Ctx.Blackboard;

        public string Name { get; }

        /// <summary>
        /// 是否在 Tick 中累加 Blackboard.StateTime。
        /// 叶子状态默认 true；父状态（组合状态）可覆写为 false，避免子状态重复计时。
        /// </summary>
        protected virtual bool TrackStateTime => true;

        protected CharacterStateBase(string name, CharacterStateContext context)
        {
            Name = name;
            Ctx = context;
        }

        public void OnEnter()
        {
            if (BB != null)
            {
                BB.StateTime = 0f;
            }

            OnStateEnter();
        }

        public void OnExit()
        {
            OnStateExit();
        }

        public void Tick(float deltaTime)
        {
            if (Ctx != null)
            {
                Ctx.DeltaTime = deltaTime;
            }

            if (BB != null && TrackStateTime)
            {
                BB.StateTime += deltaTime;
            }

            OnStateTick(deltaTime);
        }

        protected virtual void OnStateEnter() { }
        protected virtual void OnStateExit() { }
        protected virtual void OnStateTick(float deltaTime) { }
    }
}