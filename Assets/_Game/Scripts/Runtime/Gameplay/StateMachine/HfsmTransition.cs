namespace GameDemo.Runtime.Gameplay.StateMachine
{
    /// <summary>
    /// 状态转移定义：包含起点、终点、触发条件与优先级。
    /// 优先级越大，越先尝试转移（用于受击中断等强制逻辑）。
    /// </summary>
    public sealed class HfsmTransition
    {
        public string FromStateName { get; }
        public string ToStateName { get; }
        public IHfsmCondition Condition { get; }
        public int Priority { get; }

        public HfsmTransition(
            string fromStateName,
            string toStateName,
            IHfsmCondition condition,
            int priority = 0)
        {
            FromStateName = fromStateName;
            ToStateName = toStateName;
            Condition = condition;
            Priority = priority;
        }

        public bool CanTransition()
        {
            return Condition != null && Condition.Evaluate();
        }
    }
}

