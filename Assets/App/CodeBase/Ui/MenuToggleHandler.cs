using CodeBase.Infrastructure.Factories;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MenuToggleHandler : MonoBehaviour
{
    private InputActions _inputActions;
    private MenuWindow _currentWindow;
    private IUIFactory _factory;

    [Inject]
    private void Construct(IUIFactory uIFactory)
    {
        _factory = uIFactory;
    }

    private void Awake()
    {
        _inputActions = new InputActions();
    }

    private void Start()
    {
        _inputActions.Ui.ToggleMenu.performed += OnToggleMenu;
    
    }

    private void OnEnable()
    {
        _inputActions.Ui.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Ui.Disable();

    }

    private async void OnToggleMenu(InputAction.CallbackContext context)
    {
        if(_currentWindow == null)
        {
            _currentWindow = await _factory.CreateMenuWindow();
        }
    }

    private void OnDestroy()
    {
        _inputActions.Ui.ToggleMenu.performed -= OnToggleMenu;
    }
}
