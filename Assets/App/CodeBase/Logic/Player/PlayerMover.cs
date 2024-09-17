using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class PlayerMover : MonoBehaviour
    {
        [Header("Player Movement Settings")]
        [SerializeField] private float _moveSpeed = 4.0f;
        [SerializeField] private float _sprintSpeed = 6.0f;
        [SerializeField] private float _speedChangeRate = 10.0f;

        [Header("Footstep Settings")]
        [SerializeField] private AudioClip[] _footstepSounds;
        [SerializeField] private AudioSource _footStepSource;
        [SerializeField] private float _footstepInterval = 0.5f;

        private CharacterController _controller;
        private PlayerInputHandler _input;
        private PlayerJumper _playerJumper;
        private float _footstepTimer;
        private float _speed;


        private const float SpeedOffset = 0.1f;
        private const int SpeedRoundingFactor = 1000;
     
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputHandler>();
            _playerJumper = GetComponent<PlayerJumper>();
        }

        private void Update()
        {
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            float targetSpeed = GetTargetSpeed();
            UpdateSpeed(targetSpeed);

            Vector3 inputDirection = GetInputDirection();
            MovePlayer(inputDirection);
            HandleFootsteps(inputDirection);
        }

        private float GetTargetSpeed()
        {
            return _input.Sprint ? _sprintSpeed : _moveSpeed;
        }

        private void UpdateSpeed(float targetSpeed)
        {
            if (_input.Move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float inputMagnitude = _input.AnalogMovement ? _input.Move.magnitude : 1f;

            if (Mathf.Abs(currentHorizontalSpeed - targetSpeed) > SpeedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);
                _speed = Mathf.Round(_speed * SpeedRoundingFactor) / SpeedRoundingFactor;
            }
            else
            {
                _speed = targetSpeed;
            }
        }

        private Vector3 GetInputDirection()
        {
            if (_input.Move != Vector2.zero)
            {
                return transform.right * _input.Move.x + transform.forward * _input.Move.y;
            }

            return Vector3.zero;
        }

        private void MovePlayer(Vector3 inputDirection)
        {
            _controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _playerJumper.GetVerticalVelocity(), 0.0f) * Time.deltaTime);
        }
        private void HandleFootsteps(Vector3 inputDirection)
        {
            if (inputDirection != Vector3.zero && _controller.isGrounded)
            {
                _footstepTimer += Time.deltaTime;

                if (_footstepTimer >= _footstepInterval)
                {
                    PlayRandomFootstepSound();
                    _footstepTimer = 0f;
                }
            }
            else
            {
                _footstepTimer = 0f;
            }
        }

        private void PlayRandomFootstepSound()
        {
            if (_footstepSounds.Length > 0)
            {
                _footStepSource.clip = _footstepSounds[Random.Range(0, _footstepSounds.Length)];
                _footStepSource.Play();
            }
        }
    }
}
