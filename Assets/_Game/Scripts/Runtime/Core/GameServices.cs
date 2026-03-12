using GameDemo.Runtime.Gameplay.Combat;

namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// Temporary runtime access point for scene bridge components.
    /// Replace with DI container when the project grows larger.
    /// </summary>
    public static class GameServices
    {
        public static CombatSystem CombatSystem { get; private set; }

        public static void SetCombatSystem(CombatSystem combatSystem)
        {
            CombatSystem = combatSystem;
        }

        public static void Clear()
        {
            CombatSystem = null;
        }
    }
}

