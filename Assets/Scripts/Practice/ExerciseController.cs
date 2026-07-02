using UnityEngine;
using SignTrainer.Data;

namespace SignTrainer.Practice
{
    public abstract class ExerciseController : MonoBehaviour
    {
        public abstract void Begin(ExerciseData exercise);
        public abstract void SubmitAnswer(object payload);
        public abstract bool IsComplete { get; }
    }
}
