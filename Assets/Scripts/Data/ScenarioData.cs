using System.Collections.Generic;
using UnityEngine;

namespace SignTrainer.Data
{
    [CreateAssetMenu(menuName = "SignTrainer/Scenario", fileName = "SCN-NEW")]
    public class ScenarioData : ScriptableObject
    {
        [Header("Identity")]
        public string scenarioId;
        public string displayName;

        [TextArea(2, 4)] public string goalText;
        public string environmentSceneName;

        [Header("Steps (in order)")]
        public List<ScenarioStep> steps = new List<ScenarioStep>();

        [Header("Scope")]
        public bool isMvp;
    }
}
