using UnityEngine;

namespace CodeBase.Logic.Player
{
    public class PlayerJumper : MonoBehaviour
    {
        [Header("Player Jump Settings")]
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravity = -15.0f;

        [Header("Timeouts")]
        [SerializeField] private float _jumpTimeout = 0.1f;
        [SerializeField] private float _fallTimeout = 0.15f;

        [Header("Grounded Settings")]
        [SerializeField] private bool _grounded = true;
        [SerializeField] private float _groundedOffset = -0.14f;
        [SerializeField] private float _groundedRadius = 0.5f;
        [SerializeField] private LayerMask _groundLayers;

        private float _verticalVelocity;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private CharacterController _controller;
        private PlayerInputHandler _input;

        private const float TerminalVelocity = 53.0f;
        private const float DefaultVerticalVelocity = -2f;

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInputHandler>();

            _jumpTimeoutDelta = _jumpTimeout;
            _fallTimeoutDelta = _fallTimeout;
        }

        private void Update()
        {
            ProcessJumpAndGravity();
            UpdateGroundedStatus();
        }

        private void ProcessJumpAndGravity()
        {
            if (_grounded)
            {
                HandleGroundedMovement();
            }
            else
            {
                HandleAirMovement();
            }

            ApplyTerminalVelocityLimit();
        }

        private void HandleGroundedMovement()
        {
            _fallTimeoutDelta = _fallTimeout;

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = DefaultVerticalVelocity;
            }

            if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
            {
                Jump();
            }

            if (_jumpTimeoutDelta > 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }

        private void HandleAirMovement()
        {
            _jumpTimeoutDelta = _jumpTimeout;

            if (_fallTimeoutDelta > 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }

            ApplyGravity();
        }

        private void Jump()
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);
            _input.ResetJump();
        }

        private void ApplyGravity()
        {
            if (_verticalVelocity < TerminalVelocity)
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        private void ApplyTerminalVelocityLimit()
        {
            if (_verticalVelocity < TerminalVelocity)
            {
                _verticalVelocity += _gravity * Time.deltaTime;
            }
        }

        private void UpdateGroundedStatus()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);
        }

        public float GetVerticalVelocity()
        {
            return _verticalVelocity;
        }
    }
}