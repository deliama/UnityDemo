namespace GameDemo.Runtime.Gameplay.StateMachine
{
    public interface IHfsmState
    {
        string Name { get; }
        void OnEnter();
        void OnExit();
        void Tick(float deltaTime);
    }
}

