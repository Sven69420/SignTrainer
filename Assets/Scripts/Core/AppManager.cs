using UnityEngine;

namespace SignTrainer.Core
{
    public class AppManager : MonoBehaviour
    {
        public static AppManager Instance { get; private set; }

        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private string buildVersion = "0.1.0-mvp";
        [SerializeField] private bool developmentBuild = true;

        public SceneLoader SceneLoader => sceneLoader;
        public string BuildVersion => buildVersion;
        public bool IsDevelopmentBuild => developmentBuild;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            GameLogger.Log($"AppManager awake. Build {buildVersion}.");
        }

        private void Start()
        {
            if (sceneLoader == null)
            {
                GameLogger.Error("AppManager: SceneLoader reference missing.");
                return;
            }

            sceneLoader.LoadSceneAsync(SceneName.MainMenu);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
