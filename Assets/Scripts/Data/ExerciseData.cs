using System.Collections.Generic;
using UnityEngine;

namespace SignTrainer.Data
{
    [CreateAssetMenu(menuName = "SignTrainer/Exercise", fileName = "EX-NEW")]
    public class ExerciseData : ScriptableObject
    {
        public string exerciseId;
        public ExerciseType type;
        public List<SignData> signPool = new List<SignData>();

        [Min(2)] public int questionCount = 4;
    }
}
