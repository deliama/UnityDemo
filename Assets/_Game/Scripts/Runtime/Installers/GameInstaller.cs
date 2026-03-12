using GameDemo.Runtime.Configs;
using GameDemo.Runtime.Core;
using GameDemo.Runtime.Gameplay.Combat;

namespace GameDemo.Runtime.Installers
{
    /// <summary>
    /// 运行时安装器：集中创建并注册各系统，便于后续迁移到 DI 容器。
    /// </summary>
    public sealed class GameInstaller
    {
        private readonly CombatDemoConfig _combatDemoConfig;

        public GameInstaller(CombatDemoConfig combatDemoConfig)
        {
            _combatDemoConfig = combatDemoConfig;
        }

        /// <summary> 根据当前配置构建 GameLoop，并注册战斗等系统。 </summary>
        public GameLoop Build(GameContext context)
        {
            var loop = new GameLoop();
            var combatRegistry = new CombatTargetRegistry();
            var combatService = new CombatService(combatRegistry, _combatDemoConfig);
            var combatSystem = new CombatSystem(combatRegistry, combatService);

            // 供场景中的 CombatTarget 等组件通过 GameServices 访问
            GameServices.SetCombatSystem(combatSystem);
            loop.AddSystem(combatSystem);
            return loop;
        }
    }
}

