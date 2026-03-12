using GameDemo.Runtime.Configs;
using GameDemo.Runtime.Core;
using GameDemo.Runtime.Gameplay.Combat;

namespace GameDemo.Runtime.Installers
{
    /// <summary>
    /// Central installer for runtime systems.
    /// Keep scene wiring here so later migrations to DI stay simple.
    /// </summary>
    public sealed class GameInstaller
    {
        private readonly CombatDemoConfig _combatDemoConfig;

        public GameInstaller(CombatDemoConfig combatDemoConfig)
        {
            _combatDemoConfig = combatDemoConfig;
        }

        public GameLoop Build(GameContext context)
        {
            var loop = new GameLoop();
            var combatRegistry = new CombatTargetRegistry();
            var combatService = new CombatService(combatRegistry, _combatDemoConfig);
            var combatSystem = new CombatSystem(combatRegistry, combatService);

            GameServices.SetCombatSystem(combatSystem);
            loop.AddSystem(combatSystem);
            return loop;
        }
    }
}

