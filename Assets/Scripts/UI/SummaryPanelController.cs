using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SignTrainer.UI
{
    public class SummaryPanelController : MonoBehaviour
    {
        [SerializeField] private TMP_Text summaryBody;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button backButton;
        [SerializeField] private CanvasGroup group;

        public System.Action BackPressed;
        public System.Action QuitPressed;

        private void Awake()
        {
            if (quitButton != null) quitButton.onClick.AddListener(() => QuitPressed?.Invoke());
            if (backButton != null) backButton.onClick.AddListener(() => BackPressed?.Invoke());
        }

        public void Show(string body)
        {
            if (summaryBody != null) summaryBody.text = body;
            if (group != null) { group.alpha = 1f; group.interactable = true; group.blocksRaycasts = true; }
        }

        public void Hide()
        {
            if (group != null) { group.alpha = 0f; group.interactable = false; group.blocksRaycasts = false; }
        }
    }
}
