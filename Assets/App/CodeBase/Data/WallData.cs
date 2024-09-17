using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Data
{
    [System.Serializable]
    public struct WallData
    {
        public AssetReferenceGameObject WallPrefab;
        [Range(0, 100)] public float SpawnChance; // Probability of spawning
    }
}
