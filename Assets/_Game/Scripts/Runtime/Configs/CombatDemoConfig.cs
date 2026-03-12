using UnityEngine;

namespace GameDemo.Runtime.Configs
{
    /// <summary>
    /// 战斗 Demo 配置：攻击半径、扇形半角、VFX 预热数量等，数据驱动便于调参与扩展。
    /// </summary>
    [CreateAssetMenu(
        fileName = "CombatDemoConfig",
        menuName = "GameDemo/Configs/Combat Demo Config")]
    public sealed class CombatDemoConfig : ScriptableObject
    {
        [Header("Player")]
        [SerializeField] private float attackRadius = 2.5f;
        [SerializeField] private float attackHalfAngle = 45f;

        [Header("Performance")]
        [SerializeField] private int vfxWarmupCount = 10;

        /// <summary> 扇形/圆形判定半径。 </summary>
        public float AttackRadius => attackRadius;

        /// <summary> 扇形半角（度），如 45 表示 90 度扇形。 </summary>
        public float AttackHalfAngle => attackHalfAngle;

        /// <summary> 对象池预热数量，用于弹幕/特效等。 </summary>
        public int VfxWarmupCount => vfxWarmupCount;
    }
}

