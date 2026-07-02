using System.Collections.Generic;
using SignTrainer.Data;

namespace SignTrainer.Scenario
{
    public class ScenarioStepRunner
    {
        private readonly List<ScenarioStep> steps;
        private int index;

        public ScenarioStepRunner(List<ScenarioStep> steps)
        {
            this.steps = steps ?? new List<ScenarioStep>();
        }

        public ScenarioStep Current => index < steps.Count ? steps[index] : null;
        public bool IsComplete => index >= steps.Count;
        public int StepIndex => index;
        public int TotalSteps => steps.Count;

        public bool Validate(SignData submittedSign)
        {
            var step = Current;
            if (step == null || submittedSign == null) return false;
            if (step.requiredSigns == null || step.requiredSigns.Count == 0) return false;
            foreach (var req in step.requiredSigns)
            {
                if (req != null && req.signId == submittedSign.signId)
                {
                    index++;
                    return true;
                }
            }
            return false;
        }

        public void Advance() => index++;
        public void Reset() => index = 0;
    }
}
