using System.Collections;
using TMPro;
using UnityEngine;

namespace SignTrainer.Practice
{
    public enum FeedbackState { Hidden, Correct, AlmostThere, TryAgain }

    public class FeedbackController : MonoBehaviour
    {
        [SerializeField] private TMP_Text label;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float visibleSeconds = 1.5f;

        public FeedbackState Current { get; private set; } = FeedbackState.Hidden;

        public void Show(FeedbackState state)
        {
            Current = state;
            if (label != null) label.text = ToText(state);
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }

        private static string ToText(FeedbackState state) => state switch
        {
            FeedbackState.Correct => "Correct!",
            FeedbackState.AlmostThere => "Almost there — try again",
            FeedbackState.TryAgain => "Let's try once more",
            _ => string.Empty
        };

        private IEnumerator FadeIn()
        {
            if (group != null) group.alpha = 1f;
            yield return new WaitForSeconds(visibleSeconds);
            if (group != null) group.alpha = 0f;
            Current = FeedbackState.Hidden;
        }
    }
}
