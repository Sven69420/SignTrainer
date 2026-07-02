using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using SignTrainer.Data;
using SignTrainer.XR;

namespace SignTrainer.Practice
{
    /// <summary>
    /// Lets the user enter a name, watch the avatar fingerspell it, then fingerspell it
    /// themselves with live per-letter handshape validation (XR Hands, rule-based).
    /// </summary>
    public class NamePracticeController : MonoBehaviour
    {
        [System.Serializable]
        private struct LetterSign
        {
            public char letter;
            public SignData sign;
        }

        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_Text feedbackLabel;
        [SerializeField] private GameObject demoPlayerObject;
        [SerializeField] private float secondsBetweenLetters = 1.2f;
        [SerializeField] private LetterSign[] alphabetSigns;

        [Header("Fingerspelling validation")]
        [SerializeField] private HandShapeRecognizer recognizer;
        [SerializeField] private Image scoreFill;

        private Coroutine playRoutine;
        private Coroutine signRoutine;
        private bool letterConfirmed;
        private bool validating;
        private char currentLetter;

        public string CurrentName =>
            nameInput != null ? nameInput.text.Trim().ToUpperInvariant() : string.Empty;

        private void Awake()
        {
            if (recognizer == null) recognizer = FindFirstObjectByType<HandShapeRecognizer>();
            if (recognizer == null)
            {
                var go = new GameObject("HandRecognition");
                go.AddComponent<HandTracker>();
                recognizer = go.AddComponent<HandShapeRecognizer>();
            }
            recognizer.onConfirmed.AddListener(OnLetterConfirmed);
            recognizer.onScore.AddListener(OnScore);
        }

        private void OnDestroy()
        {
            if (recognizer == null) return;
            recognizer.onConfirmed.RemoveListener(OnLetterConfirmed);
            recognizer.onScore.RemoveListener(OnScore);
        }

        public void PlayName()
        {
            if (string.IsNullOrWhiteSpace(CurrentName))
            {
                ShowFeedback("Please enter your name first.");
                return;
            }
            if (playRoutine != null) StopCoroutine(playRoutine);
            playRoutine = StartCoroutine(PlayNameRoutine(CurrentName));
        }

        public void ReplayName() => PlayName();

        /// <summary>Starts the user fingerspelling their own name, validated per letter.</summary>
        public void MarkSigned()
        {
            if (string.IsNullOrWhiteSpace(CurrentName))
            {
                ShowFeedback("Enter your name first, then try signing it.");
                return;
            }
            if (signRoutine != null) StopCoroutine(signRoutine);
            signRoutine = StartCoroutine(SignNameRoutine(CurrentName));
        }

        private IEnumerator PlayNameRoutine(string name)
        {
            ShowFeedback($"Playing: {name}");
            foreach (char c in name)
            {
                if (c == ' ') continue;
                SignData sign = FindSignFor(c);
                if (sign == null) { ShowFeedback($"No sign found for letter {c}."); yield break; }
                ShowFeedback($"Letter: {c}");
                demoPlayerObject?.SendMessage("Play", sign, SendMessageOptions.DontRequireReceiver);
                yield return new WaitForSeconds(secondsBetweenLetters);
            }
            ShowFeedback("Now try signing your name yourself.");
        }

        private IEnumerator SignNameRoutine(string name)
        {
            int done = 0, total = 0;
            foreach (char c in name) if (c != ' ') total++;

            foreach (char c in name)
            {
                if (c == ' ') continue;
                var shape = Resources.Load<HandShapeData>($"HandShapes/HS_{char.ToUpperInvariant(c)}");
                if (shape == null)
                {
                    ShowFeedback($"No hand shape for '{c}' — run SignTrainer ▸ Setup ▸ Generate Hand Shapes.");
                    yield break;
                }

                currentLetter = c;
                letterConfirmed = false;
                validating = true;
                recognizer.SetTarget(shape);
                ShowFeedback($"Sign the letter  {c}   ({done + 1}/{total})");

                yield return new WaitUntil(() => letterConfirmed);

                validating = false;
                done++;
                ShowFeedback($"✓  {c}");
                if (scoreFill != null) scoreFill.fillAmount = 1f;
                yield return new WaitForSeconds(0.6f);
            }

            validating = false;
            ShowFeedback($"You signed your name!  {done}/{total} letters.");
        }

        private void OnScore(float score)
        {
            if (!validating) return;
            if (scoreFill != null) scoreFill.fillAmount = Mathf.Clamp01(score);
            ShowFeedback($"Sign  {currentLetter}    {Mathf.RoundToInt(score * 100f)}%");
        }

        private void OnLetterConfirmed() => letterConfirmed = true;

        private SignData FindSignFor(char letter)
        {
            foreach (var entry in alphabetSigns)
                if (char.ToUpperInvariant(entry.letter) == letter)
                    return entry.sign;
            return null;
        }

        private void ShowFeedback(string message)
        {
            if (feedbackLabel != null) feedbackLabel.text = message;
        }
    }
}
