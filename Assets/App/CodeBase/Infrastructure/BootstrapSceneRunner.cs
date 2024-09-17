using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Infrastructure
{
    [DefaultExecutionOrder(int.MinValue)]
    public class BootstrapSceneRunner : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();
            if (bootstrapper != null)
            {
                return;
            }

            var context = FindObjectOfType<ProjectContext>();
            if (context != null)
            {
                return;
            }

            SceneManager.LoadScene(0);
        }
#endif
    }
}