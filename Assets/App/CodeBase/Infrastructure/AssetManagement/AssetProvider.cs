using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace CodeBase.Infrastructure.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _completedCache = new Dictionary<string, AsyncOperationHandle>();
        private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();

        public void Initialize()
        {
            Addressables.InitializeAsync();
        }

        public async Task<T> Load<T>(AssetReference assetReference) where T : class
        {
            if (_completedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }

            return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(assetReference), cacheKey: assetReference.AssetGUID);
        }

        public async Task<T> Load<T>(string address) where T : class
        {
            if (_completedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
            {
                return completedHandle.Result as T;
            }

            AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);

            return await RunWithCacheOnComplete(Addressables.LoadAssetAsync<T>(address),cacheKey: address);
        }

        public async Task<List<T>> LoadAll<T>(string label) where T : class
        {
            AsyncOperationHandle<IList<T>> handle = Addressables.LoadAssetsAsync<T>(label, null);
            IList<T> results = await handle.Task;

            string cacheKey = label;
            foreach (var result in results)
            {
                _completedCache[cacheKey] = handle;
            }

            AddHandle<T>(cacheKey, handle);

            return new List<T>(results);
        }

        public void CleanUp()
        {
            foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
            {
                foreach (AsyncOperationHandle handle in resourceHandles)
                {
                    Addressables.Release(handle);
                }
            }

            _completedCache.Clear();
            _handles.Clear();
        }

        private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey) where T : class
        {
            handle.Completed += completeHandle =>
            {
                _completedCache[cacheKey] = completeHandle;
            };

            AddHandle<T>(cacheKey, handle);

            return await handle.Task;
        }

        private void AddHandle<T>(string key, AsyncOperationHandle handle) where T : class
        {
            if (!_handles.TryGetValue(key, out List<AsyncOperationHandle> resourceHandles))
            {
                resourceHandles = new List<AsyncOperationHandle>();
                _handles[key] = resourceHandles;
            }

            resourceHandles.Add(handle);
        }

        public void Release(string key)
        {
            if (!_handles.ContainsKey(key))
                return;

            foreach (var handle in _handles[key])
                Addressables.Release(handle);

            _completedCache.Remove(key);
            _handles.Remove(key);
        }

        public void Release(AssetReference assetReference)
        {
            string key = assetReference.AssetGUID;

            if (!_handles.ContainsKey(key))
                return;

            foreach (var handle in _handles[key])
                Addressables.Release(handle);

            _completedCache.Remove(key);
            _handles.Remove(key);
        }
    }
}