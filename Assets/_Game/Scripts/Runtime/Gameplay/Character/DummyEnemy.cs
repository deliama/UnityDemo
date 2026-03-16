using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Actors
{
    public sealed class DummyEnemy : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private int maxHp = 100;
        [SerializeField] private int currentHp = 100;

        [Header("Hit Reaction")]
        [SerializeField] private float hitRecoverTime = 0.35f;

        [Header("Runtime")]
        [SerializeField] private bool isHit;
        [SerializeField] private bool isDead;

        private float _hitTimer;

        public bool IsDead => isDead;
        public bool IsHit => isHit;
        public int CurrentHp => currentHp;

        private void Awake()
        {
            currentHp = maxHp;
        }

        private void Update()
        {
            if (!isHit)
            {
                return;
            }

            _hitTimer -= Time.deltaTime;
            if (_hitTimer <= 0f)
            {
                isHit = false;
                _hitTimer = 0f;
            }
        }

        public void ApplyDamage(int damage)
        {
            if (isDead)
            {
                return;
            }

            currentHp -= damage;
            currentHp = Mathf.Max(currentHp, 0);

            isHit = true;
            _hitTimer = hitRecoverTime;

            if (currentHp <= 0)
            {
                isDead = true;
                OnDead();
            }
        }

        private void OnDead()
        {
            gameObject.SetActive(false);
        }
    }
}