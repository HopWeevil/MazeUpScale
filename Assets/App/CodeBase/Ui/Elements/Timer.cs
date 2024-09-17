using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _timerText;

        private float _elapsedTime;
        private bool _isTiming;

        private void Start()
        {
            _elapsedTime = 0f;
            _isTiming = true;
        }

        private void Update()
        {
            if (_isTiming)
            {
                _elapsedTime += Time.deltaTime;
                UpdateTimerText();
            }
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt((_elapsedTime % 1) * 1000);

            _timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
        }

        public void StopTimer()
        {
            _isTiming = false;
        }

        public void ResetTimer()
        {
            _elapsedTime = 0f;
            _isTiming = true;
            UpdateTimerText();
        }
    }
}