using GameDemo.Runtime.Gameplay.Actors;
using GameDemo.Runtime.Core;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// MonoBehaviour bridge for scene actors.
    /// This keeps pure combat logic independent from GameObject details.
    /// </summary>
    public sealed class CombatTarget : MonoBehaviour, ICombatTarget
    {
        [SerializeField] private ActorTeam team = ActorTeam.Enemy;
        [SerializeField] private int maxHp = 100;

        private int _currentHp;

        public Transform CachedTransform => transform;
        public ActorTeam Team => team;
        public bool IsAlive => _currentHp > 0;

        private void Awake()
        {
            _currentHp = maxHp;
        }

        private void OnEnable()
        {
            GameServices.CombatSystem?.Registry.Register(this);
        }

        private void OnDisable()
        {
            GameServices.CombatSystem?.Registry.Unregister(this);
        }

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

