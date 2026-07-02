using UnityEngine;
using SignTrainer.Core;
using SignTrainer.Core.Buses;

namespace SignTrainer.Progress
{
    public class SessionProgressManager : MonoBehaviour
    {
        public static SessionProgressManager Instance { get; private set; }

        public SessionSnapshot Snapshot { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            Snapshot = new SessionSnapshot();
        }

        private void OnEnable()
        {
            SignCompletionBus.SignCompleted += OnSign;
            ScenarioCompletionBus.ScenarioCompleted += OnScenario;
        }

        private void OnDisable()
        {
            SignCompletionBus.SignCompleted -= OnSign;
            ScenarioCompletionBus.ScenarioCompleted -= OnScenario;
        }

        private void OnSign(string signId)
        {
            if (Snapshot.CompletedSigns.Add(signId))
                GameLogger.Log($"SessionProgress: +sign {signId} (total {Snapshot.CompletedSigns.Count})");
        }

        private void OnScenario(string scenarioId)
        {
            if (Snapshot.CompletedScenarios.Add(scenarioId))
                GameLogger.Log($"SessionProgress: +scenario {scenarioId} (total {Snapshot.CompletedScenarios.Count})");
        }

        public void MarkExerciseComplete(string exerciseId)
        {
            if (Snapshot.CompletedExercises.Add(exerciseId))
                GameLogger.Log($"SessionProgress: +exercise {exerciseId}");
        }

        public void EndSession()
        {
            Snapshot.EndTime = System.DateTime.UtcNow;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }
    }
}
