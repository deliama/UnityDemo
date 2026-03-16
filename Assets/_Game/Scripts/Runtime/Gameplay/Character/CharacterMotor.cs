using UnityEngine;
using GameDemo.Runtime.Gameplay.Cameras;

namespace GameDemo.Runtime.Gameplay.Character
{
    public sealed class CharacterMotor
    {
        private readonly Transform _owner;
        private readonly PlayerCameraController _cameraController;

        private float _moveSpeed;
        private float _rotationSpeed;

        public CharacterMotor(
            Transform owner,
            PlayerCameraController cameraController,
            float moveSpeed,
            float rotationSpeed)
        {
            _owner = owner;
            _cameraController = cameraController;
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
        }

        public void SetMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
        }

        public void SetRotationSpeed(float rotationSpeed)
        {
            _rotationSpeed = rotationSpeed;
        }

        public void Tick(Vector2 moveInput, float deltaTime)
        {
            if (_owner == null || _cameraController == null)
            {
                return;
            }

            Vector3 forward = _cameraController.ForwardOnPlane;
            Vector3 right = _cameraController.RightOnPlane;

            Vector3 moveDirection = forward * moveInput.y + right * moveInput.x;

            if (moveDirection.sqrMagnitude > 1f)
            {
                moveDirection.Normalize();
            }

            if (moveDirection.sqrMagnitude <= 0.0001f)
            {
                return;
            }

            _owner.position += moveDirection * (_moveSpeed * deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            _owner.rotation = Quaternion.RotateTowards(
                _owner.rotation,
                targetRotation,
                _rotationSpeed * deltaTime
            );
        }
    }
}