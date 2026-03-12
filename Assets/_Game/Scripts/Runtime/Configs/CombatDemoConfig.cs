using UnityEngine;

namespace GameDemo.Runtime.Configs
{
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

        public float AttackRadius => attackRadius;
        public float AttackHalfAngle => attackHalfAngle;
        public int VfxWarmupCount => vfxWarmupCount;
    }
}

