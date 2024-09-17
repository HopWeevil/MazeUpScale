using UnityEngine;

namespace CodeBase.Services.Notification
{
    public interface IPopupMessageService
    {
        void ShowMessage(string message, Color color);
    }
}