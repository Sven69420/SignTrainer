using System;

namespace SignTrainer.Core.Buses
{
    public static class ScenarioCompletionBus
    {
        public static event Action<string> ScenarioCompleted;

        public static void Raise(string scenarioId)
        {
            if (string.IsNullOrWhiteSpace(scenarioId)) return;
            ScenarioCompleted?.Invoke(scenarioId);
            GameLogger.Log($"ScenarioCompletionBus → {scenarioId}");
        }
    }
}
