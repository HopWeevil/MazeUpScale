using CodeBase.Enums;
using CodeBase.SO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;

namespace CodeBase.Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private const string MazeConfigurationLabel = "MazeConfiguration";
        private const string LevelConfigurationLabel = "LevelConfiguration";
        private const string MazeThemeLabel = "MazeTheme";

        private Dictionary<MazeSize, MazeConfigurationData> _mazeConfigurations;
        private Dictionary<string, LevelConfigurationData> _levelConfigurations;
        private Dictionary<MazeTheme, MazeThemeData> _mazeThemes;

        private readonly IAssetProvider _assetProvider;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public async Task Initialize()
        {
            _levelConfigurations = (await _assetProvider.LoadAll<LevelConfigurationData>(LevelConfigurationLabel)).ToDictionary(x => x.SceneName, x => x);
            _mazeConfigurations = (await _assetProvider.LoadAll<MazeConfigurationData>(MazeConfigurationLabel)).ToDictionary(x => x.Size, x => x);
            _mazeThemes = (await _assetProvider.LoadAll<MazeThemeData>(MazeThemeLabel)).ToDictionary(x => x.Theme, x => x);
        }

        public LevelConfigurationData ForLevel(string sceneKey)
        {
            if (_levelConfigurations.TryGetValue(sceneKey, out LevelConfigurationData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }

        public MazeConfigurationData ForMaze(MazeSize size)
        {
            if (_mazeConfigurations.TryGetValue(size, out MazeConfigurationData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }

        public MazeThemeData ForMazeTheme(MazeTheme theme)
        {

            if (_mazeThemes.TryGetValue(theme, out MazeThemeData staticData))
            {
                return staticData;
            }
            else
            {
                return null;
            }
        }

        public List<MazeConfigurationData> GetAllShips()
        {
            return _mazeConfigurations.Values.ToList();
        }      
    }
}