using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SignTrainer.Core;

namespace SignTrainer.UI
{
    public class MenuController : MonoBehaviour
    {
        [System.Serializable]
        private struct SceneOption
        {
            public string displayName;
            public SceneName sceneName;
        }

        [SerializeField] private Button tutorialButton;

        [Header("Practice Selector")]
        [SerializeField] private Button practiceButton;
        [SerializeField] private Button practicePreviousButton;
        [SerializeField] private Button practiceNextButton;
        [SerializeField] private TMP_Text practiceLabel;
        [SerializeField] private SceneOption[] practiceScenes;

        [Header("Quiz Selector")]
        [SerializeField] private Button quizButton;
        [SerializeField] private Button quizPreviousButton;
        [SerializeField] private Button quizNextButton;
        [SerializeField] private TMP_Text quizLabel;
        [SerializeField] private SceneOption[] quizScenes;

        [Header("Scenario Selector")]
        [SerializeField] private Button scenarioButton;
        [SerializeField] private Button scenarioPreviousButton;
        [SerializeField] private Button scenarioNextButton;
        [SerializeField] private TMP_Text scenarioLabel;
        [SerializeField] private SceneOption[] scenarioScenes;

        [SerializeField] private Button exitButton;

        private int practiceIndex;
        private int quizIndex;
        private int scenarioIndex;

        private void Start()
        {
            tutorialButton?.onClick.AddListener(() => Go(SceneName.Tutorial));

            practiceButton?.onClick.AddListener(() => LoadSelected(practiceScenes, practiceIndex));
            practicePreviousButton?.onClick.AddListener(() => CyclePractice(-1));
            practiceNextButton?.onClick.AddListener(() => CyclePractice(1));

            quizButton?.onClick.AddListener(() => LoadSelected(quizScenes, quizIndex));
            quizPreviousButton?.onClick.AddListener(() => CycleQuiz(-1));
            quizNextButton?.onClick.AddListener(() => CycleQuiz(1));

            scenarioButton?.onClick.AddListener(() => LoadSelected(scenarioScenes, scenarioIndex));
            scenarioPreviousButton?.onClick.AddListener(() => CycleScenario(-1));
            scenarioNextButton?.onClick.AddListener(() => CycleScenario(1));

            exitButton?.onClick.AddListener(OnExit);

            RefreshLabels();
        }

        private void CyclePractice(int direction)
        {
            practiceIndex = CycleIndex(practiceIndex, direction, practiceScenes);
            RefreshLabels();
        }

        private void CycleQuiz(int direction)
        {
            quizIndex = CycleIndex(quizIndex, direction, quizScenes);
            RefreshLabels();
        }

        private void CycleScenario(int direction)
        {
            scenarioIndex = CycleIndex(scenarioIndex, direction, scenarioScenes);
            RefreshLabels();
        }

        private static int CycleIndex(int current, int direction, SceneOption[] options)
        {
            if (options == null || options.Length == 0) return 0;
            return (current + direction + options.Length) % options.Length;
        }

        private void RefreshLabels()
        {
            if (practiceLabel != null && practiceScenes?.Length > 0)
                practiceLabel.text = practiceScenes[practiceIndex].displayName;

            if (quizLabel != null && quizScenes?.Length > 0)
                quizLabel.text = quizScenes[quizIndex].displayName;

            if (scenarioLabel != null && scenarioScenes?.Length > 0)
                scenarioLabel.text = scenarioScenes[scenarioIndex].displayName;
        }

        private void LoadSelected(SceneOption[] options, int index)
        {
            if (options == null || options.Length == 0) return;
            Go(options[index].sceneName);
        }

        private void Go(SceneName target)
        {
            if (AppManager.Instance?.SceneLoader == null) return;
            AppManager.Instance.SceneLoader.LoadSceneAsync(target);
        }

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