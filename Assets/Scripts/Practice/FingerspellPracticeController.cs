using System.Collections.Generic;
using UnityEngine;
using SignTrainer.Data;
using SignTrainer.XR;

namespace SignTrainer.Practice
{
    /// <summary>
    /// Drives a fingerspelling session: presents a target shape, waits for the
    /// recognizer to confirm it, gives feedback, then advances. Session-only scoring.
    /// </summary>
    public class FingerspellPracticeController : MonoBehaviour
    {
        [SerializeField] private HandShapeRecognizer recognizer;
        [SerializeField] private FingerspellPracticeView view;
        [SerializeField] private List<HandShapeData> shapes = new List<HandShapeData>();
        [SerializeField] private bool shuffle = true;
        [SerializeField] private float advanceDelay = 1.2f;

        private int index = -1;
        private int correctCount;

        private void Awake()
        {
            if (recognizer == null) recognizer = FindFirstObjectByType<HandShapeRecognizer>();
            if (view == null) view = FindFirstObjectByType<FingerspellPracticeView>();
            if (shapes == null || shapes.Count == 0)
                shapes = new List<HandShapeData>(Resources.LoadAll<HandShapeData>("HandShapes"));
        }

        private void Start()
        {
            if (recognizer != null)
            {
                recognizer.onConfirmed.AddListener(OnConfirmed);
                recognizer.onScore.AddListener(OnScore);
            }
            if (shuffle) Shuffle();
            Next();
        }

        private void OnDestroy()
        {
            if (recognizer == null) return;
            recognizer.onConfirmed.RemoveListener(OnConfirmed);
            recognizer.onScore.RemoveListener(OnScore);
        }

        /// <summary>Lets callers (e.g. an editor builder) supply the letter set.</summary>
        public void Configure(List<HandShapeData> set) => shapes = set;

        private void OnScore(float score) => view?.SetScore(score);

        private void Next()
        {
            index++;
            if (index >= shapes.Count)
            {
                view?.ShowComplete(correctCount, shapes.Count);
                return;
            }
            recognizer?.SetTarget(shapes[index]);
            view?.ShowPrompt(shapes[index], index + 1, shapes.Count);
        }

        private void OnConfirmed()
        {
            correctCount++;
            view?.ShowSuccess(recognizer != null ? recognizer.Target : null);
            CancelInvoke(nameof(Next));
            Invoke(nameof(Next), advanceDelay);
        }

        private void Shuffle()
        {
            for (int i = shapes.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (shapes[i], shapes[j]) = (shapes[j], shapes[i]);
            }
        }
    }
}
