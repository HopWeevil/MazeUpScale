using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;
using CodeBase.SO;
using System.Threading.Tasks;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assetProvider;


        public BootstrapState(IGameStateMachine stateMachine, IStaticDataService staticData, IAssetProvider assetProvider)
        {
            _stateMachine = stateMachine;
            _staticData = staticData;
            _assetProvider = assetProvider;
        }

        public async void Enter()
        {
            await InitializeServices();

            // A blank for selecting levels or a map generator

            LevelConfigurationData data = _staticData.ForLevel("Level1");
            _stateMachine.Enter<LoadLevelState, LevelConfigurationData>(data);
        }

        public void Exit()
        {

        }

        private async Task InitializeServices()
        {
            await _staticData.Initialize();
            _assetProvider.Initialize();
        }
    }
}