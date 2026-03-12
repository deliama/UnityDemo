using System.Collections.Generic;

namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// Lightweight runtime loop that keeps pure C# systems
    /// decoupled from MonoBehaviour lifecycle details.
    /// </summary>
    public sealed class GameLoop
    {
        private readonly List<IGameSystem> _systems = new List<IGameSystem>();
        private readonly List<ITickableSystem> _tickables = new List<ITickableSystem>();

        public void AddSystem(IGameSystem system)
        {
            if (system == null)
            {
                return;
            }

            _systems.Add(system);
            if (system is ITickableSystem tickable)
            {
                _tickables.Add(tickable);
            }
        }

        public void Initialize()
        {
            for (var i = 0; i < _systems.Count; i++)
            {
                _systems[i].Initialize();
            }
        }

        public void Tick(float deltaTime)
        {
            for (var i = 0; i < _tickables.Count; i++)
            {
                _tickables[i].Tick(deltaTime);
            }
        }

        public void LateTick(float deltaTime)
        {
            for (var i = 0; i < _tickables.Count; i++)
            {
                _tickables[i].LateTick(deltaTime);
            }
        }

        public void Shutdown()
        {
            for (var i = _systems.Count - 1; i >= 0; i--)
            {
                _systems[i].Shutdown();
            }
        }
    }
}

