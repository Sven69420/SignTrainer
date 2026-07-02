using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SignTrainer.Data;

namespace SignTrainer.Practice
{
    /// <summary>
    /// World-space UI for fingerspelling practice: prompt, live score bar, success tick + audio.
    /// Serialized refs are auto-resolved by child name if left unassigned.
    /// </summary>
    public class FingerspellPracticeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private TMP_Text hintText;
        [SerializeField] private Image scoreFill;
        [SerializeField] private GameObject checkmark;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip successClip;

        private void Awake()
        {
            if (promptText == null) promptText = FindText("Prompt");
            if (hintText == null) hintText = FindText("Hint");
            if (scoreFill == null) { var f = transform.Find("ScoreBar/Fill"); if (f) scoreFill = f.GetComponent<Image>(); }
            if (checkmark == null) { var c = transform.Find("Checkmark"); if (c) checkmark = c.gameObject; }
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            if (checkmark != null) checkmark.SetActive(false);
        }

        public void ShowPrompt(HandShapeData shape, int n, int total)
        {
            string label = shape != null ? shape.label : "?";
            if (promptText != null) promptText.text = label;
            if (hintText != null) hintText.text = $"Sign the letter  {label}    ({n}/{total})";
            if (checkmark != null) checkmark.SetActive(false);
            SetScore(0f);
        }

        public void SetScore(float value)
        {
            if (scoreFill != null) scoreFill.fillAmount = Mathf.Clamp01(value);
        }

        public void ShowSuccess(HandShapeData shape)
        {
            if (checkmark != null) checkmark.SetActive(true);
            if (audioSource != null && successClip != null) audioSource.PlayOneShot(successClip);
        }

        public void ShowComplete(int correct, int total)
        {
            if (promptText != null) promptText.text = "✓";
            if (hintText != null) hintText.text = $"Great work!   {correct}/{total} correct";
            if (checkmark != null) checkmark.SetActive(true);
            SetScore(1f);
        }

        private TMP_Text FindText(string child)
        {
            var t = transform.Find(child);
            return t != null ? t.GetComponent<TMP_Text>() : null;
        }
    }
}
