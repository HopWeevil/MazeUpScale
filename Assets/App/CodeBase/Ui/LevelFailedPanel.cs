using CodeBase.Infrastructure.States;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelFailedPanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _exitButton;

    private IGameStateMachine _gameStateMachine;
    private IStaticDataService _staticDataService;

    [Inject]
    private void Construct(IGameStateMachine gameStateMachine, IStaticDataService staticData)
    {
        _gameStateMachine = gameStateMachine;
        _staticDataService = staticData;
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartButtonClick);
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    private void OnRestartButtonClick()
    {
        LevelConfigurationData data = _staticDataService.ForLevel("Level1");
        _gameStateMachine.Enter<LoadLevelState, LevelConfigurationData>(data);
    }

    private void OnExitButtonClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Cursor.lockState = CursorLockMode.None;
#else
        Application.Quit();
#endif
    }
}
