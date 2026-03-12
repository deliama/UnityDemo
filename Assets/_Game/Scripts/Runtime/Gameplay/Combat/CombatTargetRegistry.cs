using System.Collections.Generic;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// 战斗目标注册表：维护场景中所有可被命中的目标列表，供 CombatService 做扇形/圆形查询。
    /// CombatTarget 在 OnEnable 时注册、OnDisable 时反注册。
    /// </summary>
    public sealed class CombatTargetRegistry
    {
        private readonly List<ICombatTarget> _targets = new List<ICombatTarget>();

        /// <summary> 当前已注册目标只读列表。 </summary>
        public IReadOnlyList<ICombatTarget> Targets => _targets;

        /// <summary> 注册一个目标，重复注册会被忽略。 </summary>
        public void Register(ICombatTarget target)
        {
            if (target == null || _targets.Contains(target))
            {
                return;
            }

            _targets.Add(target);
        }

        /// <summary> 从列表中移除目标（如物体禁用或销毁时）。 </summary>
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

