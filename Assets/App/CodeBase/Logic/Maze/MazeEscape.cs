using CodeBase.Infrastructure.Factories;
using CodeBase.Services.Notification;
using CodeBase.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace CodeBase.Logic.Maze
{
    public class MazeEscape : MonoBehaviour
    {
        private IPersistentProgressService _progressService;
        private IPopupMessageService _messageService;
        private IUIFactory _factory;

        [Inject]
        private void Construct(IPersistentProgressService progressService, IUIFactory uIFactory, IPopupMessageService messageService)
        {
            _progressService = progressService;
            _messageService = messageService;
            _factory = uIFactory;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerInput player))
            {
                if (_progressService.Progress.KeysCollected == _progressService.Progress.KeysToCollect)
                {
                    _factory.CreateLevelCompletePanel();
                }
                else
                {
                    _messageService.ShowMessage("Collect all the keys to open the door!", Color.white);
                }
            }
        }
    }
}