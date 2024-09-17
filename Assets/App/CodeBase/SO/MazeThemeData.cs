using CodeBase.Data;
using CodeBase.Enums;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "MazeThemeData", menuName = "Static Data/MazeTheme")]
    public class MazeThemeData : ScriptableObject
    {
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public MazeTheme Theme { get; private set; }
        [field: SerializeField] public AssetReferenceGameObject Floor { get; set; }
        [field: SerializeField] public AssetReferenceGameObject TrapFloor { get; set; }
        [field: SerializeField] public AssetReferenceGameObject Pillar { get; set; }
        [field: SerializeField] public AssetReferenceGameObject Escape { get; set; }
        [field: SerializeField] public WallData[] Walls { get; set; }
    }
}
