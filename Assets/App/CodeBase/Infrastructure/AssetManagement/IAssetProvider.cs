using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider
    {
        void Initialize();
        Task<T> Load<T>(AssetReference assetReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        Task<List<T>> LoadAll<T>(string label) where T : class;
        void Release(AssetReference assetReference);
        void Release(string key);
        void CleanUp();
    }
}