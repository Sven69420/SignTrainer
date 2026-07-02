using System;

namespace SignTrainer.Core.Buses
{
    public static class InteractionPauseBus
    {
        public static event Action Paused;
        public static event Action Resumed;
        public static bool IsPaused { get; private set; }

        public static void RaisePaused()
        {
            if (IsPaused) return;
            IsPaused = true;
            Paused?.Invoke();
            GameLogger.Log("InteractionPauseBus → Paused");
        }

        public static void RaiseResumed()
        {
            if (!IsPaused) return;
            IsPaused = false;
            Resumed?.Invoke();
            GameLogger.Log("InteractionPauseBus → Resumed");
        }
    }
}
