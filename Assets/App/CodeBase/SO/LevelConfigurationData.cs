using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "LevelConfiguration", menuName = "Static Data/LevelConfiguration")]
    public class LevelConfigurationData : ScriptableObject
    {
        [field: SerializeField] public string SceneName { get; set; }

        [field: SerializeField] public string Name { get; set; }

        [field: SerializeField] public string Description { get; set; }

        [field: SerializeField] public MazeSize MazeSize { get; set; }

        [field: SerializeField] public MazeTheme MazeTheme { get; set; }

    }
}