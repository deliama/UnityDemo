using System;

namespace GameDemo.Runtime.Gameplay.StateMachine
{
    /// <summary>
    /// 基于委托的条件实现：适合快速把业务 bool 判定接入状态机。
    /// </summary>
    public sealed class FuncCondition : IHfsmCondition
    {
        private readonly Func<bool> _predicate;

        public FuncCondition(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public bool Evaluate()
        {
            return _predicate != null && _predicate();
        }
    }
}

