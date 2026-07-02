using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands;

namespace SignTrainer.XR
{
    /// <summary>
    /// Thin wrapper over the running <see cref="XRHandSubsystem"/> that exposes
    /// per-finger extensions for either hand. Works with any OpenXR hand-tracking runtime.
    /// </summary>
    public class HandTracker : MonoBehaviour
    {
        static readonly List<XRHandSubsystem> s_Subsystems = new();
        XRHandSubsystem subsystem;

        public bool HasHands => subsystem != null && subsystem.running;

        void Update()
        {
            if (subsystem != null && subsystem.running) return;
            SubsystemManager.GetSubsystems(s_Subsystems);
            for (int i = 0; i < s_Subsystems.Count; i++)
            {
                if (s_Subsystems[i].running)
                {
                    subsystem = s_Subsystems[i];
                    break;
                }
            }
        }

        public bool TryGetExtensions(bool rightHand, out FingerExtensions ext)
        {
            ext = default;
            if (subsystem == null || !subsystem.running) return false;
            var hand = rightHand ? subsystem.rightHand : subsystem.leftHand;
            return HandCurl.TryFromHand(hand, out ext);
        }
    }
}
