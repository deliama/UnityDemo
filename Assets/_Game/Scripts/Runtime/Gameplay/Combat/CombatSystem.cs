using GameDemo.Runtime.Core;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// 战斗域系统：持有目标注册表与战斗服务，作为 GameLoop 中的战斗入口。
    /// 当前仅做占位；后续可扩展：命中队列、伤害管线、受击打断等。
    /// </summary>
    public sealed class CombatSystem : IGameSystem, ITickableSystem
    {
        private readonly CombatTargetRegistry _registry;
        private readonly CombatService _combatService;

        public CombatSystem(CombatTargetRegistry registry, CombatService combatService)
        {
            _registry = registry;
            _combatService = combatService;
        }

        /// <summary> 目标注册表，供 CombatTarget 的 OnEnable/OnDisable 使用。 </summary>
        public CombatTargetRegistry Registry => _registry;

        /// <summary> 战斗查询与配置，供玩家/技能逻辑做扇形判定与伤害结算。 </summary>
        public CombatService CombatService => _combatService;

        /// <summary> 占位：后续可在此预加载战斗资源或注册全局事件。 </summary>
        public void Initialize()
        {
        }

        /// <summary> 占位：后续可在此处理命中检测、技能 CD 等。 </summary>
        public void Tick(float deltaTime)
        {
        }

        /// <summary> 占位：后续可在此统一结算伤害、派发受击事件。 </summary>
        public void LateTick(float deltaTime)
        {
        }

        /// <summary> 占位：后续可在此清理战斗相关引用与监听。 </summary>
        public void Shutdown()
        {
        }
    }
}

