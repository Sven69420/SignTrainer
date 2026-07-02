using UnityEngine;
using SignTrainer.Core;

namespace SignTrainer.UI
{
    public class SceneButtonLoader : MonoBehaviour
    {
        [SerializeField] private SceneName targetScene = SceneName.MainMenu;

        public void LoadTarget()
        {
            if (AppManager.Instance == null || AppManager.Instance.SceneLoader == null)
            {
                GameLogger.Error("SceneButtonLoader: AppManager or SceneLoader missing.");
                return;
            }

            AppManager.Instance.SceneLoader.LoadSceneAsync(targetScene);
        }
    }
}