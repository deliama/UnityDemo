using System.Collections.Generic;

namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// 轻量级运行时循环：统一驱动纯 C# 系统，与 MonoBehaviour 生命周期解耦。
    /// 系统按注册顺序 Initialize / Tick / LateTick，Shutdown 时逆序释放。
    /// </summary>
    public sealed class GameLoop
    {
        private readonly List<IGameSystem> _systems = new List<IGameSystem>();
        private readonly List<ITickableSystem> _tickables = new List<ITickableSystem>();

        /// <summary> 注册一个系统；若实现 ITickableSystem 则同时加入每帧 Tick。 </summary>
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

        /// <summary> 按注册顺序初始化所有系统。 </summary>
        public void Initialize()
        {
            for (var i = 0; i < _systems.Count; i++)
            {
                _systems[i].Initialize();
            }
        }

        /// <summary> 每帧调用一次，驱动所有可 Tick 系统。 </summary>
        public void Tick(float deltaTime)
        {
            for (var i = 0; i < _tickables.Count; i++)
            {
                _tickables[i].Tick(deltaTime);
            }
        }

        /// <summary> 每帧 LateUpdate 阶段调用。 </summary>
        public void LateTick(float deltaTime)
        {
            for (var i = 0; i < _tickables.Count; i++)
            {
                _tickables[i].LateTick(deltaTime);
            }
        }

        /// <summary> 逆序关闭所有系统，释放资源。 </summary>
        public void Shutdown()
        {
            for (var i = _systems.Count - 1; i >= 0; i--)
            {
                _systems[i].Shutdown();
            }
        }
    }
}

