namespace GameDemo.Runtime.Gameplay.StateMachine
{
    /// <summary>
    /// 分层状态机单状态接口：用于 Idle / Move / Attack / Hit 等状态的实现。
    /// </summary>
    public interface IHfsmState
    {
        /// <summary> 状态唯一标识，用于 ChangeState(stateName)。 </summary>
        string Name { get; }

        /// <summary> 进入该状态时调用。 </summary>
        void OnEnter();

        /// <summary> 离开该状态时调用。 </summary>
        void OnExit();

        /// <summary> 处于该状态时每帧调用。 </summary>
        void Tick(float deltaTime);
    }
}

