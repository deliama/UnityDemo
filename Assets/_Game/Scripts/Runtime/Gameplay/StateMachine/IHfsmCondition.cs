namespace GameDemo.Runtime.Gameplay.StateMachine
{
    /// <summary>
    /// 状态转移条件接口：用于描述“何时允许从一个状态切到另一个状态”。
    /// </summary>
    public interface IHfsmCondition
    {
        /// <summary> 返回 true 表示当前帧允许转移。 </summary>
        bool Evaluate();
    }
}

