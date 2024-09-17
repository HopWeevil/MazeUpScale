using Cinemachine;
using CodeBase.Data;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Sceneloader;
using CodeBase.Logic.Maze;
using CodeBase.Logic.Player;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using CodeBase.UI.Curtain;
using CodeBase.UI.Elements;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<LevelConfigurationData>
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly ILoadingCurtain _loadingCurtain;
        private readonly IGameFactory _gameFactory;
        private readonly IUIFactory _uIFactory;
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _progressService;
        private LevelConfigurationData _levelConfiguration;

        public LoadLevelState(
            IGameStateMachine gameStateMachine,
            ISceneLoader sceneLoader,
            ILoadingCurtain loadingCurtain,
            IGameFactory gameFactory,
            IUIFactory uIFactory,
            IStaticDataService staticData,
            IPersistentProgressService progressService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _loadingCurtain = loadingCurtain;
            _gameFactory = gameFactory;
            _uIFactory = uIFactory;
            _staticData = staticData;
            _progressService = progressService;
        }

        public void Enter(LevelConfigurationData configuration)
        {
            _levelConfiguration = configuration;

            _loadingCurtain.Show();
            _uIFactory.WarmUp();
            _gameFactory.WarmUp();

            _sceneLoader.Load(_levelConfiguration.SceneName, OnLoadedAsync);
        }

        public void Exit()
        {
            _loadingCurtain.Hide();
            _uIFactory.CleanUp();
            _gameFactory.CleanUp();
        }

        private async void OnLoadedAsync()
        {
            await InitUiRoot();
            await InitGameWorld();
            _stateMachine.Enter<GameLoopState>();
        }

        private async Task InitGameWorld()
        {
            LevelConfigurationData data = _staticData.ForLevel(_levelConfiguration.SceneName);
            MazeConfigurationData mazeConfiguration = _staticData.ForMaze(data.MazeSize);
            MazeThemeData themeData = _staticData.ForMazeTheme(data.MazeTheme);

            MazeGeneratorResult result = await CreateMaze(mazeConfiguration, themeData);
            CreateNewProgress(result);

            GameObject player = await CreatePlayer(result.PlayerSpawnPosition);
            await InitHud(player);

        }

        private async Task<MazeGeneratorResult> CreateMaze(MazeConfigurationData mazeConfiguration, MazeThemeData themeData)
        {
            MazeBuilder builder = new MazeBuilder(_gameFactory, themeData);
            MazeGenerator generator = new MazeGenerator(mazeConfiguration, themeData);
            MazeGeneratorResult result = generator.Generate();
            await builder.Build(result);
            return result;
        }

        private void CreateNewProgress(MazeGeneratorResult result)
        {
            _progressService.Progress = new PlayerProgress(result.Keys.Count);
        }

        private async Task<GameObject> CreatePlayer(Vector3 at)
        {
            return await _gameFactory.CreatePlayer(at);
        }

        private void CameraFollow(Transform target)
        {
            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
            brain.ActiveVirtualCamera.Follow = target;
        }

        private async Task InitHud(GameObject player)
        {
            GameObject hud = await _uIFactory.CreateHud();
            hud.GetComponentInChildren<ActorUI>().Construct(player.GetComponent<PlayerHealth>());
        }

        private async Task InitUiRoot()
        {
            await _uIFactory.CreateUIRoot();
        }
    }
}