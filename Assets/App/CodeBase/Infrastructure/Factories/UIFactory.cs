using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;
using CodeBase.UI.Elements;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CodeBase.Infrastructure.Factories
{
    public class UIFactory : IUIFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly DiContainer _container;
        private Canvas _uiRoot;

        public UIFactory(DiContainer container, IAssetProvider assets, IStaticDataService staticData)
        {
            _assets = assets;
            _staticData = staticData;
            _container = container;
        }

        public async Task WarmUp()
        {
            await _assets.Load<GameObject>(AssetAddress.PopupMessage);
            await _assets.Load<GameObject>(AssetAddress.LevelCompletePanel);
            await _assets.Load<GameObject>(AssetAddress.LevelFailedPanel);
        }

        public void CleanUp()
        {
            _assets.CleanUp();
        }

        public async Task<PopupMessage> CreatePopupMessage(Color color, string text)
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.PopupMessage);
            PopupMessage message = Object.Instantiate(prefab, _uiRoot.transform).GetComponent<PopupMessage>();
            message.SetColor(color);
            message.SetText(text);
            return message;
        }

        public async Task CreateUIRoot()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.UIRootPath);
            _uiRoot = Object.Instantiate(prefab).GetComponent<Canvas>();
        }

        public async Task<GameObject> CreateHud()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.HudPath);
            GameObject hud = Object.Instantiate(prefab);
            _container.InjectGameObject(hud);
            return hud;
        }

        public async Task<LevelCompletePanel> CreateLevelCompletePanel()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.LevelCompletePanel);
            GameObject window = _container.InstantiatePrefab(prefab, _uiRoot.transform);
            return window.GetComponent<LevelCompletePanel>();
        }

        public async Task<LevelFailedPanel> CreateLevelFailedPanel()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.LevelFailedPanel);
            GameObject window = _container.InstantiatePrefab(prefab, _uiRoot.transform);
            return window.GetComponent<LevelFailedPanel>();
        }

        public async Task<MenuWindow> CreateMenuWindow()
        {
            GameObject prefab = await _assets.Load<GameObject>(AssetAddress.MenuWindow);
            GameObject window = _container.InstantiatePrefab(prefab, _uiRoot.transform);
            return window.GetComponent<MenuWindow>();
        }
    }
}