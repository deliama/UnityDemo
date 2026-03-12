using GameDemo.Runtime.Gameplay.Combat;

namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// 运行时服务访问点：供场景桥接组件（如 CombatTarget）获取战斗系统等。
    /// 项目扩大后可改为依赖注入容器，避免静态全局。
    /// </summary>
    public static class GameServices
    {
        /// <summary> 当前战斗系统，用于目标注册与查询。 </summary>
        public static CombatSystem CombatSystem { get; private set; }

        public static void SetCombatSystem(CombatSystem combatSystem)
        {
            CombatSystem = combatSystem;
        }

        /// <summary> 场景卸载时清空引用，避免悬空。 </summary>
        public static void Clear()
        {
            CombatSystem = null;
        }
    }
}

