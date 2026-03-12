using GameDemo.Runtime.Gameplay.Actors;
using GameDemo.Runtime.Core;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// 场景战斗目标桥接：挂在玩家/敌人 GameObject 上，实现 ICombatTarget，
    /// 在 OnEnable 时注册到战斗系统，OnDisable 时反注册，保持纯逻辑与场景解耦。
    /// </summary>
    public sealed class CombatTarget : MonoBehaviour, ICombatTarget
    {
        [SerializeField] private ActorTeam team = ActorTeam.Enemy;
        [SerializeField] private int maxHp = 100;

        private int _currentHp; // 当前血量，Awake 时赋为 maxHp

        public Transform CachedTransform => transform;
        public ActorTeam Team => team;
        public bool IsAlive => _currentHp > 0;

        private void Awake()
        {
            _currentHp = maxHp;
        }

        /// <summary> 启用时加入战斗注册表，参与扇形/圆形判定。 </summary>
        private void OnEnable()
        {
            GameServices.CombatSystem?.Registry.Register(this);
        }

        /// <summary> 禁用时从注册表移除，避免悬空引用。 </summary>
        private void OnDisable()
        {
            GameServices.CombatSystem?.Registry.Unregister(this);
        }

        /// <summary> 承受伤害，由战斗逻辑调用；已死亡则忽略。 </summary>
        public void ApplyHit(int damage)
        {
            if (!IsAlive)
            {
                return;
            }

            _currentHp = Mathf.Max(0, _currentHp - Mathf.Max(0, damage));
        }
    }
}

