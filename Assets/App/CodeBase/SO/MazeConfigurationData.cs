using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.SO
{
    [CreateAssetMenu(fileName = "MazeConfigurationData", menuName = "Static Data/MazeConfiguration")]
    public class MazeConfigurationData : ScriptableObject
    {
        [field: SerializeField] public MazeSize Size { get; private set; }
        [field: SerializeField] public bool FullRandom { get; set; } = false;
        [field: SerializeField] public int RandomSeed { get; set; } = 12345;
        [field: SerializeField] public int Rows { get; set; } = 5;
        [field: SerializeField] public int Columns { get; set; } = 5;
        [field: SerializeField] public float CellWidth { get; set; } = 5f;
        [field: SerializeField] public float CellHeight { get; set; } = 5f;
        [field: SerializeField] public int MinKeys { get; set; } = 1;
        [field: SerializeField] public int MaxKeys { get; set; } = 5;
        [field: SerializeField] public int MinDistanceFromExit { get; set; } = 5;
        [field: SerializeField] public int MaxDistanceFromExit { get; set; } = 7;
        [field: SerializeField] public float FloorTrapsChance { get; set; } = 0.05f;
    }
}