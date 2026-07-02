using System;
using System.Collections.Generic;
using UnityEngine;

namespace SignTrainer.Data
{
    [Serializable]
    public class ScenarioStep
    {
        public StepType type;
        public List<SignData> requiredSigns = new List<SignData>();
        [TextArea(2, 3)] public string hintText;
        [Tooltip("Used only when type == Await")] public float awaitSeconds;
    }
}
