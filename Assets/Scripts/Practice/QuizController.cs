using System.Collections.Generic;
using UnityEngine;
using SignTrainer.Core;
using SignTrainer.Data;

namespace SignTrainer.Practice
{
    public class QuizController : ExerciseController
    {
        private const int ChoicesPerQuestion = 4;

        private ExerciseData current;
        private int questionIndex;
        private int correctCount;
        private readonly List<SignData> questions = new List<SignData>();

        public int TotalQuestions => current?.questionCount ?? 0;
        public int CurrentQuestionIndex => questionIndex;
        public int CorrectCount => correctCount;
        public override bool IsComplete => current != null && questionIndex >= TotalQuestions;

        public override void Begin(ExerciseData exercise)
        {
            current = exercise;
            questionIndex = 0;
            correctCount = 0;
            questions.Clear();
            if (exercise?.signPool == null) return;
            int n = Mathf.Min(exercise.questionCount, exercise.signPool.Count);
            for (int i = 0; i < n; i++) questions.Add(exercise.signPool[Random.Range(0, exercise.signPool.Count)]);
            GameLogger.Log($"Quiz: {questions.Count} questions prepared");
        }

        public SignData CurrentTarget => IsComplete ? null : questions[questionIndex];

        public List<SignData> BuildChoicesFor(SignData target)
        {
            var choices = new List<SignData> { target };
            if (current?.signPool == null) return choices;
            var pool = new List<SignData>(current.signPool);
            pool.RemoveAll(s => s == null || s.signId == target.signId);
            while (choices.Count < ChoicesPerQuestion && pool.Count > 0)
            {
                int i = Random.Range(0, pool.Count);
                choices.Add(pool[i]);
                pool.RemoveAt(i);
            }
            Shuffle(choices);
            return choices;
        }

        public override void SubmitAnswer(object payload)
        {
            if (payload is not SignData selection || current == null || IsComplete) return;
            var target = CurrentTarget;
            if (target != null && selection.signId == target.signId) correctCount++;
            questionIndex++;
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
