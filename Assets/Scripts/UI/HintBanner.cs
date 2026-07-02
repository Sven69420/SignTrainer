using System.Collections;
using TMPro;
using UnityEngine;

namespace SignTrainer.UI
{
    public class HintBanner : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float autoHideSeconds = 4f;

        private Coroutine hideRoutine;

        public void ShowHint(string text)
        {
            if (label != null) label.text = text;
            Show();
            if (hideRoutine != null) StopCoroutine(hideRoutine);
            hideRoutine = StartCoroutine(AutoHide());
        }

        private IEnumerator AutoHide()
        {
            yield return new WaitForSeconds(autoHideSeconds);
            Hide();
        }

        private void Show()
        {
            if (group == null) return;
            group.alpha = 1f;
        }

        private void Hide()
        {
            if (group == null) return;
            group.alpha = 0f;
        }
    }
}
