using System.Collections.Generic;

namespace GameDemo.Runtime.Gameplay.StateMachine
{
    /// <summary>
    /// Minimal hierarchical state machine entry.
    /// Can be extended with transition guards and interrupt priorities.
    /// </summary>
    public sealed class HfsmMachine
    {
        private readonly Dictionary<string, IHfsmState> _states = new Dictionary<string, IHfsmState>();
        private IHfsmState _current;

        public string CurrentStateName => _current?.Name ?? "None";

        public void Register(IHfsmState state)
        {
            if (state == null || string.IsNullOrWhiteSpace(state.Name))
            {
                return;
            }

            _states[state.Name] = state;
        }

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

        public void Tick(float deltaTime)
        {
            _current?.Tick(deltaTime);
        }
    }
}

