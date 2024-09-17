using CodeBase.Services.PersistentProgress;
using TMPro;
using UnityEngine;
using Zenject;

namespace CodeBase.UI.Elements
{
    public class KeysCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _counter;

        private IPersistentProgressService _progressService;
        private string _format;

        [Inject]
        private void Construct(IPersistentProgressService progressService)
        {
            _progressService = progressService;
        }

        private void OnEnable()
        {
           
        }

        private void Awake()
        {
            SetFormat();
        }

        private void Start()
        {
         
            _progressService.Progress.KeyCollected += OnKeyCollected;
            OnKeyCollected();
        }

        private void OnDestroy()
        {
            _progressService.Progress.KeyCollected -= OnKeyCollected;
        }

        private void SetFormat()
        {
            _format = _counter.text;
        }

        private void OnKeyCollected()
        {
            _counter.text = string.Format(_format, _progressService.Progress.KeysCollected.ToString(), _progressService.Progress.KeysToCollect.ToString());
        }
    }
}