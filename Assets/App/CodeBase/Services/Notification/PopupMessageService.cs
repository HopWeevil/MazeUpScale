using CodeBase.Infrastructure.Factories;
using CodeBase.UI.Elements;
using UnityEngine;

namespace CodeBase.Services.Notification
{
    public class PopupMessageService : IPopupMessageService
    {
        private readonly IUIFactory _uiFactory;
        private PopupMessage _currentPopupMessage;

        public PopupMessageService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public async void ShowMessage(string message, Color color)
        {
            if (_currentPopupMessage != null)
            {
                _currentPopupMessage.HideImmediate();
            }

            PopupMessage popupMessage = await _uiFactory.CreatePopupMessage(color, message);

            _currentPopupMessage = popupMessage;
            popupMessage.Show();
        }
    }
}
