using GameDemo.Runtime.Gameplay.Actors;
using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    public interface ICombatTarget
    {
        Transform CachedTransform { get; }
        ActorTeam Team { get; }
        bool IsAlive { get; }
        void ApplyHit(int damage);
    }
}

