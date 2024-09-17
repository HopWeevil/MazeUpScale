using UnityEngine;
using CodeBase.Infrastructure.Factories;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBase.Logic.Maze
{
    public class MazeBuilder
    {
        private IStaticDataService _staticData;
        private IGameFactory _gameFactory;
        private MazeThemeData _mazeThemeData;

        private const string ParentName = "Maze";
        private Transform _parent;

        public MazeBuilder(IGameFactory gameFactory, MazeThemeData data)
        {
            _parent = new GameObject(ParentName).transform;
            _gameFactory = gameFactory;
            _mazeThemeData = data;
        }

        public async Task Build(MazeGeneratorResult result)
        {
            await CreateWalls(result);
            await CreateFloor(result);
            await CreateTrapFloor(result);
            await CreateMazeEscape(result);
            await CreatePillars(result);
            await CreateKeys(result);
        }

        private async Task CreateFloor(MazeGeneratorResult result)
        {
            for (int i = 0; i < result.FloorTiles.Count; i++)
            {
                await _gameFactory.Create(_mazeThemeData.Floor, result.FloorTiles[i], Quaternion.identity, _parent);
            }
        }

        private async Task CreateTrapFloor(MazeGeneratorResult result)
        {
            for (int i = 0; i < result.TrapFloorTiles.Count; i++)
            {
                await _gameFactory.Create(_mazeThemeData.TrapFloor, result.TrapFloorTiles[i], Quaternion.identity, _parent);
            }
        }

        private async Task CreatePillars(MazeGeneratorResult result)
        {
            for (int i = 0; i < result.Pillars.Count; i++)
            {
                await _gameFactory.Create(_mazeThemeData.Pillar, result.Pillars[i], Quaternion.identity.normalized, _parent);
            }
        }

        private async Task CreateMazeEscape(MazeGeneratorResult result)
        {
            await _gameFactory.CreateInjected(_mazeThemeData.Escape, result.Escape.Item1, result.Escape.Item2, _parent);
        }

        private async Task CreateKeys(MazeGeneratorResult result)
        {
            for (int i = 0; i < result.Keys.Count; i++)
            {
                await _gameFactory.CreateKey(result.Keys[i], _parent);
            }
        }

        private async Task CreateWalls(MazeGeneratorResult result)
        {
            foreach (var item in result.Walls)
            {
                Vector3 position = item.Item1;
                Quaternion rotation = item.Item2;
                await CreateRandomWall(position, rotation, _parent);
            }
        }

        private async Task<GameObject> CreateRandomWall(Vector3 at, Quaternion rotation, Transform parent)
        {
            float totalChance = _mazeThemeData.Walls.Sum(w => w.SpawnChance);

            float randomValue = Random.Range(0, totalChance);
            float cumulativeChance = 0f;

            foreach (var wallData in _mazeThemeData.Walls)
            {
                cumulativeChance += wallData.SpawnChance;
                if (randomValue <= cumulativeChance)
                {
                    return await _gameFactory.Create(wallData.WallPrefab, at, rotation, parent);
                }
            }

            return null;
        }
    }
}