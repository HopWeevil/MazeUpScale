using CodeBase.UI.Elements;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public interface IUIFactory
    {
        Task CreateUIRoot();
        Task<GameObject> CreateHud();
        Task<PopupMessage> CreatePopupMessage(Color color, string text);
        Task WarmUp();
        void CleanUp();
        Task<LevelCompletePanel> CreateLevelCompletePanel();
        Task<LevelFailedPanel> CreateLevelFailedPanel();
        Task<MenuWindow> CreateMenuWindow();
    }
}