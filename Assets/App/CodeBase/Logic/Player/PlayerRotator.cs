using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class PlayerRotator : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private GameObject _cinemachineCameraTarget;
        [SerializeField] private float _rotationSpeed = 1.0f;
        [SerializeField] private float _topClamp = 90.0f;
        [SerializeField] private float _bottomClamp = -90.0f;

        private float _cinemachineTargetPitch;
        private float _rotationVelocity;
        private PlayerInputHandler _input;

        private const float MinMoveThreshold = 0.01f;

        private void Start()
        {
            _input = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            ProcessCameraRotation();
        }

        private void ProcessCameraRotation()
        {
            if (_input.Look.sqrMagnitude >= MinMoveThreshold)
            {
                float deltaTimeMultiplier = _input.IsMouseInput() ? 1.0f : Time.deltaTime;
                UpdateCinemachineTargetPitch(deltaTimeMultiplier);
                UpdateRotationVelocity(deltaTimeMultiplier);

                ApplyCameraRotation();
                RotatePlayer();
            }
        }

        private void UpdateCinemachineTargetPitch(float deltaTimeMultiplier)
        {
            _cinemachineTargetPitch += _input.Look.y * _rotationSpeed * deltaTimeMultiplier;
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
        }

        private void UpdateRotationVelocity(float deltaTimeMultiplier)
        {
            _rotationVelocity = _input.Look.x * _rotationSpeed * deltaTimeMultiplier;
        }

        private void ApplyCameraRotation()
        {
            _cinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);
        }

        private void RotatePlayer()
        {
            transform.Rotate(Vector3.up * _rotationVelocity);
        }

        private static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360f) angle += 360f;
            if (angle > 360f) angle -= 360f;
            return Mathf.Clamp(angle, min, max);
        }
    }
}