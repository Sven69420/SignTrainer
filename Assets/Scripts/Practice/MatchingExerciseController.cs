using System.Collections.Generic;
using UnityEngine;
using SignTrainer.Core;
using SignTrainer.Data;

namespace SignTrainer.Practice
{
    public class MatchingExerciseController : ExerciseController
    {
        private readonly HashSet<string> pairedIds = new HashSet<string>();
        private ExerciseData current;
        private int targetPairs;

        public override bool IsComplete => current != null && pairedIds.Count >= targetPairs;

        public override void Begin(ExerciseData exercise)
        {
            current = exercise;
            pairedIds.Clear();
            targetPairs = Mathf.Min(exercise?.questionCount ?? 4, exercise?.signPool?.Count ?? 4);
            GameLogger.Log($"MatchingExercise: target {targetPairs} pairs");
        }

        public override void SubmitAnswer(object payload)
        {
            if (payload is not (SignData left, SignData right)) return;
            if (left == null || right == null) return;
            if (left.signId == right.signId) pairedIds.Add(left.signId);
        }
    }
}
