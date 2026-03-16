using UnityEngine;
using UnityEngine.InputSystem;

namespace GameDemo.Runtime.Gameplay.Cameras
{
    public sealed class PlayerCameraController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;
        [SerializeField] private Camera controlledCamera;
        [SerializeField] private InputActionAsset inputActions;

        [Header("Distance")]
        [SerializeField] private float distance = 4.5f;
        [SerializeField] private float heightOffset = 0.0f;

        [Header("Rotation")]
        [SerializeField] private float yawSensitivity = 0.12f;
        [SerializeField] private float pitchSensitivity = 0.10f;
        [SerializeField] private float minPitch = -20f;
        [SerializeField] private float maxPitch = 60f;

        [Header("Runtime")]
        [SerializeField] private float yaw;
        [SerializeField] private float pitch = 15f;

        private InputAction _lookAction;

        public Vector3 ForwardOnPlane
        {
            get
            {
                Vector3 forward = controlledCamera != null
                    ? controlledCamera.transform.forward
                    : transform.forward;

                forward.y = 0f;
                return forward.sqrMagnitude > 0.0001f ? forward.normalized : Vector3.forward;
            }
        }

        public Vector3 RightOnPlane
        {
            get
            {
                Vector3 right = controlledCamera != null
                    ? controlledCamera.transform.right
                    : transform.right;

                right.y = 0f;
                return right.sqrMagnitude > 0.0001f ? right.normalized : Vector3.right;
            }
        }

        private void Awake()
        {
            if (controlledCamera == null)
            {
                controlledCamera = Camera.main;
            }

            if (inputActions != null)
            {
                InputActionMap gameplayMap = inputActions.FindActionMap("GamePlay", true);
                _lookAction = gameplayMap.FindAction("Look", true);
                _lookAction.Enable();
            }

            Vector3 euler = transform.eulerAngles;
            yaw = euler.y;
        }

        private void OnDestroy()
        {
            _lookAction?.Disable();
        }

        private void LateUpdate()
        {
            if (target == null || controlledCamera == null)
            {
                return;
            }

            UpdateRotation();
            UpdateCameraTransform();
        }

        private void UpdateRotation()
        {
            if (_lookAction == null)
            {
                return;
            }

            Vector2 lookDelta = _lookAction.ReadValue<Vector2>();

            yaw += lookDelta.x * yawSensitivity;
            pitch -= lookDelta.y * pitchSensitivity;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        private void UpdateCameraTransform()
        {
            Vector3 targetPosition = target.position + Vector3.up * heightOffset;

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

            controlledCamera.transform.position = targetPosition + offset;
            controlledCamera.transform.LookAt(targetPosition);
        }
    }
}