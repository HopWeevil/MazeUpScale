using System;
using System.Collections;

namespace CodeBase.Infrastructure.Sceneloader
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLoaded = null);
        IEnumerator LoadScene(string nextScene, Action onLoaded = null);
    }
}