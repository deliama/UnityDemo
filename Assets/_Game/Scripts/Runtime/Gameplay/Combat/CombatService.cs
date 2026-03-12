using System.Collections.Generic;
using GameDemo.Runtime.Configs;
using GameDemo.Runtime.Gameplay.Actors;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    public sealed class CombatService
    {
        private readonly CombatTargetRegistry _registry;
        private readonly CombatDemoConfig _config;

        public CombatService(CombatTargetRegistry registry, CombatDemoConfig config)
        {
            _registry = registry;
            _config = config;
        }

        public CombatDemoConfig Config => _config;

        public void QuerySectorTargets(
            Vector3 origin,
            Vector3 forward,
            ActorTeam attackerTeam,
            List<ICombatTarget> results)
        {
            results.Clear();

            var targets = _registry.Targets;
            for (var i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                if (target == null || !target.IsAlive || target.Team == attackerTeam)
                {
                    continue;
                }

                if (HitTestUtility.IsPointInSector(
                        origin,
                        forward,
                        _config.AttackRadius,
                        _config.AttackHalfAngle,
                        target.CachedTransform.position))
                {
                    results.Add(target);
                }
            }
        }
    }
}

