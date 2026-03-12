using System.Collections.Generic;

namespace GameDemo.Runtime.Gameplay.Combat
{
    public sealed class CombatTargetRegistry
    {
        private readonly List<ICombatTarget> _targets = new List<ICombatTarget>();

        public IReadOnlyList<ICombatTarget> Targets => _targets;

        public void Register(ICombatTarget target)
        {
            if (target == null || _targets.Contains(target))
            {
                return;
            }

            _targets.Add(target);
        }

        public void Unregister(ICombatTarget target)
        {
            if (target == null)
            {
                return;
            }

            _targets.Remove(target);
        }
    }
}

