using UnityEngine;
using UnityEngine.Events;
using SignTrainer.Data;

namespace SignTrainer.XR
{
    /// <summary>
    /// Continuously scores the user's hand against a target shape and fires
    /// <see cref="onConfirmed"/> once the shape is held for <see cref="holdSeconds"/>.
    /// </summary>
    public class HandShapeRecognizer : MonoBehaviour
    {
        [SerializeField] private HandTracker tracker;
        [SerializeField] private bool rightHand = true;
        [SerializeField, Range(0.1f, 2f)] private float holdSeconds = 0.6f;

        [System.Serializable] public class ScoreEvent : UnityEvent<float> { }
        public ScoreEvent onScore = new ScoreEvent();
        public UnityEvent onConfirmed = new UnityEvent();

        public HandShapeData Target { get; private set; }
        public float CurrentScore { get; private set; }
        public PoseVerdict CurrentVerdict { get; private set; } = PoseVerdict.TryAgain;

        private float held;
        private bool confirmed;

        private void Awake()
        {
            if (tracker == null) tracker = FindFirstObjectByType<HandTracker>();
        }

        public void SetTarget(HandShapeData target)
        {
            Target = target;
            held = 0f;
            confirmed = false;
            CurrentScore = 0f;
            CurrentVerdict = PoseVerdict.TryAgain;
        }

        private void Update()
        {
            if (Target == null || tracker == null) return;
            if (!tracker.TryGetExtensions(rightHand, out var ext))
            {
                held = 0f;
                return;
            }

            CurrentScore = HandShapeMatcher.Score(ext, Target);
            CurrentVerdict = HandShapeMatcher.Verdict(CurrentScore);
            onScore.Invoke(CurrentScore);

            if (CurrentVerdict == PoseVerdict.Correct)
            {
                held += Time.deltaTime;
                if (!confirmed && held >= holdSeconds)
                {
                    confirmed = true;
                    onConfirmed.Invoke();
                }
            }
            else
            {
                held = 0f;
            }
        }
    }
}
