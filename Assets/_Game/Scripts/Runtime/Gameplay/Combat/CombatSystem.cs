using GameDemo.Runtime.Core;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// Runtime owner for combat domain objects.
    /// Start simple now; later add hit queue, damage pipeline, and rollback-friendly data.
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

        public CombatTargetRegistry Registry => _registry;
        public CombatService CombatService => _combatService;

        public void Initialize()
        {
        }

        public void Tick(float deltaTime)
        {
        }

        public void LateTick(float deltaTime)
        {
        }

        public void Shutdown()
        {
        }
    }
}

