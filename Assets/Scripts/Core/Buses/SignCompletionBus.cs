using System;

namespace SignTrainer.Core.Buses
{
    public static class SignCompletionBus
    {
        public static event Action<string> SignCompleted;

        public static void Raise(string signId)
        {
            if (string.IsNullOrWhiteSpace(signId)) return;
            SignCompleted?.Invoke(signId);
            GameLogger.Log($"SignCompletionBus → {signId}");
        }
    }
}
