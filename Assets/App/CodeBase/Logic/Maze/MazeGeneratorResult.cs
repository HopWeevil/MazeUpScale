using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Logic.Maze 
{
    public class MazeGeneratorResult
    {
        public Vector3 PlayerSpawnPosition { get; set; } = Vector3.zero;
        public List<Vector3> FloorTiles { get; set; } = new List<Vector3>();
        public List<Vector3> TrapFloorTiles { get; set; } = new List<Vector3>();
        public HashSet<(Vector3, Quaternion)> Walls { get; set; } = new HashSet<(Vector3, Quaternion)>();
        public (Vector3, Quaternion) Escape { get; set; } = (Vector3.zero, Quaternion.identity);
        public List<Vector3> Keys { get; set; } = new List<Vector3>();
        public List<Vector3> Pillars { get; set; } = new List<Vector3>();
    }
}
