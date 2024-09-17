using CodeBase.Enums;
using CodeBase.SO;
using System.Threading.Tasks;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService
    {
        Task Initialize();
        MazeConfigurationData ForMaze(MazeSize size);
        MazeThemeData ForMazeTheme(MazeTheme theme);
        LevelConfigurationData ForLevel(string sceneKey);
    }
}