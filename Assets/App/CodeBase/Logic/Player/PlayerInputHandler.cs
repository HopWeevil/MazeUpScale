using UnityEngine;
using UnityEngine.InputSystem;

namespace CodeBase.Logic.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [field: SerializeField] public Vector2 Move { get; private set; }
        [field: SerializeField] public Vector2 Look { get; private set; }
        [field: SerializeField] public bool Jump { get; private set; }
        [field: SerializeField] public bool Sprint { get; private set; }
        [field: SerializeField] public bool AnalogMovement { get; private set; }

        [SerializeField] private bool _cursorLocked = true;
        [SerializeField] private bool _cursorInputForLook = true;
        [SerializeField] private PlayerInput _input;

        private void Start()
        {
            SetCursorState(_cursorLocked);
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (_cursorInputForLook && Time.timeScale != 0)
                LookInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public bool IsMouseInput()
        {
            return _input.currentControlScheme == "KeyboardMouse";
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            Move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            Look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            Jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            Sprint = newSprintState;
        }

        public void ResetJump()
        {
            Jump = false;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (Time.timeScale != 0)
            {
                SetCursorState(_cursorLocked);
            }
        }

        public void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
