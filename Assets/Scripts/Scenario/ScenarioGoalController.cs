using TMPro;
using UnityEngine;

namespace SignTrainer.Scenario
{
    public class ScenarioGoalController : MonoBehaviour
    {
        [SerializeField] private TMP_Text goalLabel;
        [SerializeField] private CanvasGroup group;

        public void ShowGoal(string goalText)
        {
            if (goalLabel != null) goalLabel.text = goalText;
            if (group != null) { group.alpha = 1f; group.blocksRaycasts = false; }
        }

        public void Hide()
        {
            if (group != null) group.alpha = 0f;
        }
    }
}
