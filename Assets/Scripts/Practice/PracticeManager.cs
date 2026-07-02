using UnityEngine;
using SignTrainer.Core;
using SignTrainer.Core.Buses;
using SignTrainer.Data;

namespace SignTrainer.Practice
{
    public enum PracticeState { Idle, ExerciseChosen, Playing, Completed }

    public class PracticeManager : MonoBehaviour
    {
        [SerializeField] private FeedbackController feedback;
        [SerializeField] private ExerciseData defaultExercise;

        public PracticeState State { get; private set; } = PracticeState.Idle;
        public ExerciseData CurrentExercise { get; private set; }
        public int RetryCount { get; private set; }

        private void Start()
        {
            if (defaultExercise != null) BeginExercise(defaultExercise);
        }

        public void BeginExercise(ExerciseData exercise)
        {
            if (exercise == null) return;
            CurrentExercise = exercise;
            SetState(PracticeState.ExerciseChosen);
            SetState(PracticeState.Playing);
        }

        public void RecordAttempt(bool correct)
        {
            if (!correct)
            {
                RetryCount++;
                feedback?.Show(FeedbackState.TryAgain);
                return;
            }
            feedback?.Show(FeedbackState.Correct);
            SetState(PracticeState.Completed);
            if (CurrentExercise != null) SignCompletionBus.Raise(CurrentExercise.exerciseId);
        }

        private void SetState(PracticeState next)
        {
            GameLogger.Log($"PracticeManager: {State} → {next}");
            State = next;
        }
    }
}
