using System;
using System.Collections.Generic;

namespace SignTrainer.Progress
{
    [Serializable]
    public class SessionSnapshot
    {
        public HashSet<string> CompletedSigns = new HashSet<string>();
        public HashSet<string> CompletedExercises = new HashSet<string>();
        public HashSet<string> CompletedScenarios = new HashSet<string>();
        public DateTime StartTime = DateTime.UtcNow;
        public DateTime? EndTime;

        public TimeSpan Duration =>
            (EndTime ?? DateTime.UtcNow) - StartTime;
    }
}
