using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuWindow : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Slider _volumeSlider;

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(ResumeGame);
        _resumeButton.onClick.AddListener(ResumeGame);
        _exitButton.onClick.AddListener(ExitGame);
        _volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(ResumeGame);
        _resumeButton.onClick.RemoveListener(ResumeGame);
        _exitButton.onClick.RemoveListener(ExitGame);
    }

    private void Awake()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }

    private void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        Cursor.lockState = CursorLockMode.None;
#else
        Application.Quit();
#endif
    }

    private void OnSliderValueChanged(float value)
    {
        AudioListener.volume = value;
    }


}
