using Game.Gameplay.StateMachine.Character;

namespace GameDemo.Runtime.Gameplay.StateMachine.Character
{
    public abstract class CharacterStateBase : IHfsmState
    {
        protected CharacterStateContext Ctx { get; }
        protected CharacterStateBlackboard BB => Ctx.Blackboard;

        public string Name { get; }

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

            if (BB != null)
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