using GameDemo.Runtime.Gameplay.Actors;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// 战斗目标接口：可被扇形/圆形等判定命中，并承受伤害。
    /// 场景中的 CombatTarget 组件实现此接口并注册到 CombatTargetRegistry。
    /// </summary>
    public interface ICombatTarget
    {
        /// <summary> 用于判定时的位置（如扇形原点、距离）。 </summary>
        Transform CachedTransform { get; }

        /// <summary> 阵营，同阵营不参与命中。 </summary>
        ActorTeam Team { get; }

        /// <summary> 是否仍存活，死亡目标不参与判定。 </summary>
        bool IsAlive { get; }

        /// <summary> 施加伤害（由战斗逻辑调用）。 </summary>
        void ApplyHit(int damage);
    }
}

