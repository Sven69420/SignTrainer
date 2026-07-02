using UnityEngine;
using SignTrainer.Core;
using SignTrainer.Core.Buses;
using SignTrainer.Data;

namespace SignTrainer.Scenario
{
    public enum ScenarioState { Loading, GoalPresented, AwaitingStep, StepValidated, Completing, Completed }

    public class ScenarioManager : MonoBehaviour
    {
        [SerializeField] private ScenarioData scenario;
        [SerializeField] private ScenarioGoalController goalController;
        [SerializeField] private NPCResponseController npcController;

        public ScenarioState State { get; private set; } = ScenarioState.Loading;
        private ScenarioStepRunner runner;

        private void Start()
        {
            if (scenario == null) { GameLogger.Error("ScenarioManager: scenario not set"); return; }
            runner = new ScenarioStepRunner(scenario.steps);
            goalController?.ShowGoal(scenario.goalText);
            SetState(ScenarioState.GoalPresented);
            SetState(ScenarioState.AwaitingStep);
        }

        public void SubmitSign(SignData sign)
        {
            if (runner == null || State != ScenarioState.AwaitingStep) return;
            if (runner.Validate(sign))
            {
                SetState(ScenarioState.StepValidated);
                npcController?.PlayResponse(sign.signId);
                if (runner.IsComplete) CompleteScenario();
                else SetState(ScenarioState.AwaitingStep);
            }
        }

        private void CompleteScenario()
        {
            SetState(ScenarioState.Completing);
            if (scenario != null) ScenarioCompletionBus.Raise(scenario.scenarioId);
            SetState(ScenarioState.Completed);
        }

        private void SetState(ScenarioState next)
        {
            GameLogger.Log($"ScenarioManager: {State} → {next}");
            State = next;
        }
    }
}
