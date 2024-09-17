using UnityEngine;
using System.Collections.Generic;
using CodeBase.SO;
using System.Linq;

namespace CodeBase.Logic.Maze
{
    public class MazeGenerator
    {
        private MazeGeneratorResult _result;
        private List<CellToVisit> _cellsToVisit;
        private MazeCell[,] _maze;

        private MazeConfigurationData _config;
        private MazeThemeData _theme;

        public MazeGenerator(MazeConfigurationData config, MazeThemeData theme)
        {
            _config = config;
            _theme = theme;
            _result = new MazeGeneratorResult();
            _cellsToVisit = new List<CellToVisit>();
        }

        public MazeGeneratorResult Generate()
        {
            InitSeed();
            Initialize();
            GenerateMaze();
            PlaceEscape();
            SpawnMazeElements();
            SpawnPillars();
            PlaceKeysInMaze();
            GetPlayerSpawnPosition();
            return _result;
        }

        private void InitSeed()
        {
            if (!_config.FullRandom)
            {
                Random.InitState(_config.RandomSeed);
            }
        }

        private void Initialize()
        {
            _maze = new MazeCell[_config.Rows, _config.Columns];
            for (int r = 0; r < _config.Rows; r++)
            {
                for (int c = 0; c < _config.Columns; c++)
                {
                    _maze[r, c] = new MazeCell();
                }
            }
        }

        private void GenerateMaze()
        {
            _cellsToVisit.Add(new CellToVisit(Random.Range(0, _config.Rows), Random.Range(0, _config.Columns), Direction.Start));
            while (_cellsToVisit.Count > 0)
            {
                var currentCell = GetNextCell();
                var moves = GetAvailableMoves(currentCell, out int moveCount);
                ProcessCell(currentCell, moves, moveCount);
            }
        }

        private CellToVisit GetNextCell() => _cellsToVisit[Random.Range(0, _cellsToVisit.Count)];

        private Direction[] GetAvailableMoves(CellToVisit cell, out int moveCount)
        {
            var moves = new List<Direction>();
            AddMoveIfValid(cell, Direction.Right, 0, 1, moves);
            AddMoveIfValid(cell, Direction.Front, 1, 0, moves);
            AddMoveIfValid(cell, Direction.Left, 0, -1, moves);
            AddMoveIfValid(cell, Direction.Back, -1, 0, moves);

            moveCount = moves.Count;
            return moves.ToArray();
        }

        private void AddMoveIfValid(CellToVisit cell, Direction direction, int dRow, int dCol, List<Direction> moves)
        {
            int newRow = cell.Row + dRow, newCol = cell.Column + dCol;
            if (IsWithinBounds(newRow, newCol) && !_maze[newRow, newCol].IsVisited && !_cellsToVisit.Exists(c => c.Row == newRow && c.Column == newCol))
            {
                moves.Add(direction);
            }
            else if (!_maze[cell.Row, cell.Column].IsVisited && cell.MoveMade != OppositeDirection(direction))
            {
                SetWall(cell, direction);
            }
        }

        private bool IsWithinBounds(int row, int col) => row >= 0 && row < _config.Rows && col >= 0 && col < _config.Columns;

        private void SetWall(CellToVisit cell, Direction direction)
        {
            switch (direction)
            {
                case Direction.Right: _maze[cell.Row, cell.Column].WallRight = true; break;
                case Direction.Front: _maze[cell.Row, cell.Column].WallFront = true; break;
                case Direction.Left: _maze[cell.Row, cell.Column].WallLeft = true; break;
                case Direction.Back: _maze[cell.Row, cell.Column].WallBack = true; break;
            }
        }

        private Direction OppositeDirection(Direction move) => move switch
        {
            Direction.Right => Direction.Left,
            Direction.Left => Direction.Right,
            Direction.Front => Direction.Back,
            Direction.Back => Direction.Front,
            _ => Direction.Start
        };

        private void ProcessCell(CellToVisit cell, Direction[] moves, int moveCount)
        {
            _maze[cell.Row, cell.Column].IsVisited = true;
            if (moveCount == 0)
            {
                _cellsToVisit.Remove(cell);
            }
            else
            {
                var move = moves[Random.Range(0, moveCount)];
                AddNextCell(cell, move);
            }
        }

        private void AddNextCell(CellToVisit cell, Direction move)
        {
            (int newRow, int newCol) = move switch
            {
                Direction.Right => (cell.Row, cell.Column + 1),
                Direction.Front => (cell.Row + 1, cell.Column),
                Direction.Left => (cell.Row, cell.Column - 1),
                Direction.Back => (cell.Row - 1, cell.Column),
                _ => (cell.Row, cell.Column)
            };
            _cellsToVisit.Add(new CellToVisit(newRow, newCol, move));
        }

        private void SpawnMazeElements()
        {
            for (int row = 0; row < _config.Rows; row++)
            {
                for (int col = 0; col < _config.Columns; col++)
                {
                    var position = new Vector3(col * _config.CellWidth, 0, row * _config.CellHeight);
                    var cell = _maze[row, col];

                    SpawnFloor(position);
                    SpawnWalls(cell, position);
                }
            }
        }

        private void SpawnFloor(Vector3 position)
        {
            if (Random.value < _config.FloorTrapsChance)
            {
                _result.TrapFloorTiles.Add(position);
            }
            else
            {
                _result.FloorTiles.Add(position);
            }
        }

        private void SpawnWalls(MazeCell cell, Vector3 position)
        {
            TrySpawnWall(cell.WallRight, position + Vector3.right * _config.CellWidth / 2, Quaternion.Euler(0, 90, 0));
            TrySpawnWall(cell.WallFront, position + Vector3.forward * _config.CellHeight / 2, Quaternion.identity);
            TrySpawnWall(cell.WallLeft, position + Vector3.left * _config.CellWidth / 2, Quaternion.Euler(0, 270, 0));
            TrySpawnWall(cell.WallBack, position + Vector3.back * _config.CellHeight / 2, Quaternion.Euler(0, 180, 0));
        }

        private void TrySpawnWall(bool hasWall, Vector3 wallPos, Quaternion rotation)
        {
            if (hasWall && !IsExitWallPosition(wallPos, rotation))
            {
                bool wallExists = _result.Walls.Any(wall => wall.Item1 == wallPos);

                if (!wallExists)
                {
                    _result.Walls.Add((wallPos, rotation));
                }
            }
        }

        private void PlaceEscape()
        {
            var exitCoords = GetRandomExitCoords(out Direction exitDirection);
            var exitPosition = GetWallPosition(exitCoords.x, exitCoords.y, exitDirection);
            var exitRotation = GetWallRotation(exitDirection);

            _result.Escape = (exitPosition, exitRotation);
        }

        private Vector2Int GetRandomExitCoords(out Direction exitDirection)
        {
            int row, col;
            if (Random.Range(0, 2) == 0)
            {
                row = Random.Range(0, _config.Rows);
                col = Random.Range(0, 2) == 0 ? 0 : _config.Columns - 1;
                exitDirection = col == 0 ? Direction.Left : Direction.Right;
            }
            else
            {
                col = Random.Range(0, _config.Columns);
                row = Random.Range(0, 2) == 0 ? 0 : _config.Rows - 1;
                exitDirection = row == 0 ? Direction.Back : Direction.Front;
            }
            return new Vector2Int(row, col);
        }

        private Vector3 GetWallPosition(int row, int col, Direction direction)
        {
            var pos = new Vector3(col * _config.CellWidth, 0, row * _config.CellHeight);
            return direction switch
            {
                Direction.Left => pos + Vector3.left * _config.CellWidth / 2,
                Direction.Right => pos + Vector3.right * _config.CellWidth / 2,
                Direction.Front => pos + Vector3.forward * _config.CellHeight / 2,
                Direction.Back => pos + Vector3.back * _config.CellHeight / 2,
                _ => pos
            };
        }

        private Vector3 GetRandomCellPosition()
        {
            int row = Random.Range(0, _config.Rows);
            int column = Random.Range(0, _config.Columns);
            return new Vector3(column * _config.CellWidth, 0, row * _config.CellHeight);
        }


        private void PlaceKeysInMaze()
        {
            int keyCount = Random.Range(_config.MinKeys, _config.MaxKeys + 1);

            for (int i = 0; i < keyCount; i++)
            {
                Vector3 keyPosition = GetRandomCellPosition();
                _result.Keys.Add(keyPosition + Vector3.up);
            }
        }

        private void SpawnPillars()
        {
            for (int row = 0; row <= _config.Rows; row++)
            {
                for (int column = 0; column <= _config.Columns; column++)
                {
                    Vector3 pillarPosition = new Vector3(column * _config.CellWidth - _config.CellWidth / 2, 0, row * _config.CellHeight - _config.CellHeight / 2);
                    _result.Pillars.Add(pillarPosition);
                }
            }
        }

        private void GetPlayerSpawnPosition()
        {
            Vector3 playerPosition;
            int distanceFromExit;
            do
            {
                playerPosition = GetRandomCellPosition();
                distanceFromExit = Mathf.RoundToInt(Vector3.Distance(playerPosition, _result.Escape.Item1) / _config.CellWidth);
            }
            while (distanceFromExit < _config.MinDistanceFromExit || distanceFromExit > _config.MaxDistanceFromExit);

            _result.PlayerSpawnPosition = playerPosition + Vector3.up;
        }

        private Quaternion GetWallRotation(Direction direction) => direction switch
        {
            Direction.Left => Quaternion.Euler(0, 270, 0),
            Direction.Right => Quaternion.Euler(0, 90, 0),
            Direction.Front => Quaternion.identity,
            Direction.Back => Quaternion.Euler(0, 180, 0),
            _ => Quaternion.identity
        };

        private bool IsExitWallPosition(Vector3 wallPos, Quaternion rotation) => _result.Escape == (wallPos, rotation);
    }
}