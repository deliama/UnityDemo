using System.Collections.Generic;

namespace GameDemo.Runtime.Gameplay.StateMachine
{
    /// <summary>
    /// 分层状态机：按状态名切换，驱动当前状态的 OnEnter / OnExit / Tick。
    /// 后续可扩展：转换条件、受击中断优先级、子状态等。
    /// </summary>
    public sealed class HfsmMachine
    {
        private readonly Dictionary<string, IHfsmState> _states = new Dictionary<string, IHfsmState>();
        private IHfsmState _current;

        /// <summary> 当前状态名，无则为 "None"。 </summary>
        public string CurrentStateName => _current?.Name ?? "None";

        /// <summary> 注册一个状态，以 state.Name 为键。 </summary>
        public void Register(IHfsmState state)
        {
            if (state == null || string.IsNullOrWhiteSpace(state.Name))
            {
                return;
            }

            _states[state.Name] = state;
        }

        /// <summary> 切换到指定状态；若已是该状态则仅返回 true。 </summary>
        public bool ChangeState(string stateName)
        {
            if (!_states.TryGetValue(stateName, out var next))
            {
                return false;
            }

            if (_current == next)
            {
                return true;
            }

            _current?.OnExit();
            _current = next;
            _current.OnEnter();
            return true;
        }

        /// <summary> 每帧驱动当前状态逻辑。 </summary>
        public void Tick(float deltaTime)
        {
            _current?.Tick(deltaTime);
        }
    }
}

