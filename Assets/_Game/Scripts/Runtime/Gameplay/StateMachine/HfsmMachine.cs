using System.Collections.Generic;

namespace GameDemo.Runtime.Gameplay.StateMachine
{
    /// <summary>
    /// HFSM 核心状态机：支持默认状态、条件转移、AnyState 转移和优先级中断。
    /// 适用于角色状态（Idle/Move/Attack/Hit）与受击打断等逻辑。
    /// </summary>
    public sealed class HfsmMachine
    {
        private readonly Dictionary<string, IHfsmState> _states = new Dictionary<string, IHfsmState>();
        private readonly List<string> _stateOrder = new List<string>();
        private readonly List<HfsmTransition> _transitions = new List<HfsmTransition>();
        private readonly List<HfsmTransition> _anyTransitions = new List<HfsmTransition>();

        private IHfsmState _current;
        private string _defaultStateName;
        private bool _isRunning;

        /// <summary> 当前状态名，无则为 "None"。 </summary>
        public string CurrentStateName => _current?.Name ?? "None";
        public IHfsmState CurrentState => _current;
        public bool IsRunning => _isRunning;

        /// <summary> 注册一个状态，以 state.Name 为键。 </summary>
        public void Register(IHfsmState state)
        {
            if (state == null || string.IsNullOrWhiteSpace(state.Name))
            {
                return;
            }

            if (!_states.ContainsKey(state.Name))
            {
                _stateOrder.Add(state.Name);
            }

            _states[state.Name] = state;
        }

        /// <summary> 设置默认状态，Start() 未指定起始状态时会使用它。 </summary>
        public bool SetDefaultState(string stateName)
        {
            if (string.IsNullOrWhiteSpace(stateName) || !_states.ContainsKey(stateName))
            {
                return false;
            }

            _defaultStateName = stateName;
            return true;
        }

        /// <summary> 添加常规转移（From -> To）。 </summary>
        public void AddTransition(string fromStateName, string toStateName, IHfsmCondition condition, int priority = 0)
        {
            if (string.IsNullOrWhiteSpace(fromStateName) || string.IsNullOrWhiteSpace(toStateName))
            {
                return;
            }

            _transitions.Add(new HfsmTransition(fromStateName, toStateName, condition, priority));
        }

        /// <summary> 添加 AnyState 转移（任意状态 -> To）。 </summary>
        public void AddAnyTransition(string toStateName, IHfsmCondition condition, int priority = 0)
        {
            if (string.IsNullOrWhiteSpace(toStateName))
            {
                return;
            }

            _anyTransitions.Add(new HfsmTransition(string.Empty, toStateName, condition, priority));
        }

        /// <summary> 启动状态机；优先使用 startStateName，其次默认状态，再其次首个注册状态。 </summary>
        public bool Start(string startStateName = null)
        {
            var initial = ResolveInitialStateName(startStateName);
            if (string.IsNullOrWhiteSpace(initial))
            {
                return false;
            }

            _isRunning = true;
            return ChangeStateInternal(initial);
        }

        /// <summary> 停止状态机并退出当前状态。 </summary>
        public void Stop()
        {
            if (!_isRunning)
            {
                return;
            }

            _current?.OnExit();
            _current = null;
            _isRunning = false;
        }

        /// <summary> 手动切换到指定状态；若已是该状态则仅返回 true。 </summary>
        public bool ChangeState(string stateName)
        {
            if (!_isRunning)
            {
                _isRunning = true;
            }

            return ChangeStateInternal(stateName);
        }

        /// <summary> 每帧先尝试条件转移，再驱动当前状态逻辑。 </summary>
        public void Tick(float deltaTime)
        {
            if (!_isRunning)
            {
                return;
            }

            TryAutoTransition();
            _current?.Tick(deltaTime);
        }

        private bool ChangeStateInternal(string stateName)
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

        private void TryAutoTransition()
        {
            if (_current == null)
            {
                return;
            }

            var anyTransition = FindBestTransition(_anyTransitions, null);
            var stateTransition = FindBestTransition(_transitions, _current.Name);
            var target = SelectHigherPriority(anyTransition, stateTransition);

            if (target == null || string.IsNullOrWhiteSpace(target.ToStateName))
            {
                return;
            }

            ChangeStateInternal(target.ToStateName);
        }

        private HfsmTransition FindBestTransition(List<HfsmTransition> list, string fromStateName)
        {
            HfsmTransition best = null;

            for (var i = 0; i < list.Count; i++)
            {
                var transition = list[i];
                if (transition == null)
                {
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(fromStateName) &&
                    transition.FromStateName != fromStateName)
                {
                    continue;
                }

                if (!_states.ContainsKey(transition.ToStateName) || !transition.CanTransition())
                {
                    continue;
                }

                if (best == null || transition.Priority > best.Priority)
                {
                    best = transition;
                }
            }

            return best;
        }

        private HfsmTransition SelectHigherPriority(HfsmTransition left, HfsmTransition right)
        {
            if (left == null)
            {
                return right;
            }

            if (right == null)
            {
                return left;
            }

            return left.Priority >= right.Priority ? left : right;
        }

        private string ResolveInitialStateName(string startStateName)
        {
            if (!string.IsNullOrWhiteSpace(startStateName) && _states.ContainsKey(startStateName))
            {
                return startStateName;
            }

            if (!string.IsNullOrWhiteSpace(_defaultStateName) && _states.ContainsKey(_defaultStateName))
            {
                return _defaultStateName;
            }

            return _stateOrder.Count > 0 ? _stateOrder[0] : null;
        }
    }
}

