using UnityEngine;
using UnityEngine.UI;
using SignTrainer.Core;

namespace SignTrainer.UI
{
    public class MenuController_BAK : MonoBehaviour
    {
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button practiceButton;
        [SerializeField] private Button quizButton;
        [SerializeField] private Button scenarioButton;
        [SerializeField] private Button exitButton;

        private void Start()
        {
            if (tutorialButton != null) tutorialButton.onClick.AddListener(() => Go(SceneName.Tutorial));
            if (practiceButton != null) practiceButton.onClick.AddListener(() => Go(SceneName.Practice));
            quizButton?.onClick.AddListener(() => AppManager.Instance.SceneLoader.LoadSceneAsync(SceneName.PracticeQuiz));
            if (scenarioButton != null) scenarioButton.onClick.AddListener(OnScenario);
            if (exitButton != null) exitButton.onClick.AddListener(OnExit);
        }

        private void Go(SceneName target)
        {
            if (AppManager.Instance?.SceneLoader == null) return;
            AppManager.Instance.SceneLoader.LoadSceneAsync(target);
        }

        private void OnScenario() => Go(SceneName.Scenario_Greeting);

        private void OnExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
