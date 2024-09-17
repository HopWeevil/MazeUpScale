using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enums;
using CodeBase.SO;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.Factories
{
    public interface IGameFactory
    {
        Task WarmUp();
        void CleanUp();
        Task<GameObject> CreatePlayer(Vector3 at);
        Task<GameObject> Create(AssetReferenceGameObject asset, Vector3 at, Quaternion rotation, Transform parent);
        Task<GameObject> CreateKey(Vector3 at, Transform parent);
        Task<GameObject> CreateInjected(AssetReferenceGameObject asset, Vector3 at, Quaternion rotation, Transform parent);
    }
}