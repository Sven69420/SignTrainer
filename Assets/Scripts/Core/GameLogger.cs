using System.Diagnostics;
using UnityEngine;

namespace SignTrainer.Core
{
    public static class GameLogger
    {
        private const string Prefix = "[SignTrainer]";

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Log(string message) => UnityEngine.Debug.Log($"{Prefix} {message}");

        [Conditional("DEVELOPMENT_BUILD"), Conditional("UNITY_EDITOR")]
        public static void Warn(string message) => UnityEngine.Debug.LogWarning($"{Prefix} {message}");

        public static void Error(string message) => UnityEngine.Debug.LogError($"{Prefix} {message}");
    }
}
