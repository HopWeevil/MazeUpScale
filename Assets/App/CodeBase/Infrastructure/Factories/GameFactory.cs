using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;
using CodeBase.Services.Randomizer;
using System.Threading.Tasks;
using UnityEngine;
using CodeBase.SO;
using Zenject;
using UnityEngine.AddressableAssets;
using System.Linq;

namespace CodeBase.Infrastructure.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IStaticDataService _staticData;
        private readonly IRandomService _randomService;
        private readonly DiContainer _container;
   
        public GameFactory(DiContainer container, IAssetProvider assets, IStaticDataService staticDataService, IRandomService randomService)
        {
            _assets = assets;
            _staticData = staticDataService;
            _randomService = randomService;
            _container = container;
        }

        public async Task WarmUp()
        {
          
        }

        public void CleanUp()
        {
            _assets.CleanUp();
        }

        public async Task<GameObject> Create(AssetReferenceGameObject asset, Vector3 at, Quaternion rotation, Transform parent)
        {
            GameObject loadedAsset = await _assets.Load<GameObject>(asset);
            return Object.Instantiate(loadedAsset, at, rotation, parent);
        }

        public async Task<GameObject> CreateInjected(AssetReferenceGameObject asset, Vector3 at, Quaternion rotation, Transform parent)
        {
            GameObject loadedAsset = await _assets.Load<GameObject>(asset);
            return _container.InstantiatePrefab(loadedAsset, at, rotation, parent);
        }

        public async Task<GameObject> CreatePlayer(Vector3 at)
        {       
            GameObject playerAsset = await _assets.Load<GameObject>(AssetAddress.Player);
            GameObject player = Object.Instantiate(playerAsset, at, Quaternion.identity);
            _container.InjectGameObject(player);
            return player;
        }

        public async Task<GameObject> CreateKey(Vector3 at, Transform parent)
        {
            GameObject key = await _assets.Load<GameObject>(AssetAddress.Key);
            return _container.InstantiatePrefab(key, at, Quaternion.identity, parent);
        }
    }
}