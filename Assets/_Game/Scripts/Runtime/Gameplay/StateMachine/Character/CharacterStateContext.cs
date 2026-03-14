using GameDemo.Runtime.Gameplay.StateMachine.Character;
using UnityEngine;

namespace Game.Gameplay.StateMachine.Character
{
    public sealed class CharacterStateContext
    {
        public GameObject Owner;
        public Transform Transform;
        public CharacterStateBlackboard Blackboard;
        public float DeltaTime;
    }
}