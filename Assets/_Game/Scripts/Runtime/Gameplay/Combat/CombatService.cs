using System.Collections.Generic;
using GameDemo.Runtime.Configs;
using GameDemo.Runtime.Gameplay.Actors;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// 战斗查询服务：根据配置的半径、角度等，从注册表中筛选出扇形/圆形内的敌方目标。
    /// 使用手写 HitTestUtility，不依赖 Collider。
    /// </summary>
    public sealed class CombatService
    {
        private readonly CombatTargetRegistry _registry;
        private readonly CombatDemoConfig _config;

        public CombatService(CombatTargetRegistry registry, CombatDemoConfig config)
        {
            _registry = registry;
            _config = config;
        }

        /// <summary> 当前战斗相关配置（攻击半径、半角等）。 </summary>
        public CombatDemoConfig Config => _config;

        /// <summary>
        /// 查询扇形范围内的有效目标，写入 results（会先 Clear）。
        /// 排除同阵营与已死亡目标。
        /// </summary>
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

