using UnityEngine;
using UnityEngine.InputSystem;

namespace GameDemo.Runtime.Gameplay.Character
{
    public sealed class CharacterInputAdapter
    {
        public Vector2 MoveInput { get; private set; }
        public bool HitPressedThisFrame { get; private set; }
        public bool AttackPressedThisFrame {get; private set;}

        private readonly InputAction _moveAction;
        private readonly InputAction _hitDebugAction;
        private readonly InputAction _attackAction;

        public CharacterInputAdapter(InputActionAsset inputActions)
        {
            var gameplayMap = inputActions.FindActionMap("GamePlay", throwIfNotFound: true);
            _moveAction = gameplayMap.FindAction("Move", throwIfNotFound: true);
            _hitDebugAction = gameplayMap.FindAction("HitDebug", throwIfNotFound: true);
            _attackAction = gameplayMap.FindAction("Attack", throwIfNotFound: true);

            _moveAction.Enable();
            _hitDebugAction.Enable();
            _attackAction.Enable();
        }

        public void Tick()
        {
            MoveInput = _moveAction.ReadValue<Vector2>();
            HitPressedThisFrame = _hitDebugAction.WasPressedThisFrame();
            AttackPressedThisFrame = _attackAction.WasPressedThisFrame();
        }

        public void Dispose()
        {
            _moveAction.Disable();
            _hitDebugAction.Disable();
            _attackAction.Disable();
        }
    }
}