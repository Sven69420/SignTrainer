using TMPro;
using UnityEngine;
using SignTrainer.Core.Buses;

namespace SignTrainer.UI
{
    public class ProgressPanelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text signsLabel;
        [SerializeField] private TMP_Text scenariosLabel;
        [SerializeField] private int totalMvpSigns = 12;
        [SerializeField] private int totalMvpScenarios = 2;

        private int signsCompleted;
        private int scenariosCompleted;

        private void OnEnable()
        {
            SignCompletionBus.SignCompleted += OnSign;
            ScenarioCompletionBus.ScenarioCompleted += OnScenario;
            Refresh();
        }

        private void OnDisable()
        {
            SignCompletionBus.SignCompleted -= OnSign;
            ScenarioCompletionBus.ScenarioCompleted -= OnScenario;
        }

        private void OnSign(string _) { signsCompleted++; Refresh(); }
        private void OnScenario(string _) { scenariosCompleted++; Refresh(); }

        private void Refresh()
        {
            if (signsLabel != null) signsLabel.text = $"Signs: {signsCompleted}/{totalMvpSigns}";
            if (scenariosLabel != null) scenariosLabel.text = $"Scenarios: {scenariosCompleted}/{totalMvpScenarios}";
        }
    }
}
