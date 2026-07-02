using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using SignTrainer.Core;
using SignTrainer.Core.Buses;

namespace SignTrainer.XR
{
    public class TrackingMonitor : MonoBehaviour
    {
        [SerializeField] private float lossGraceSeconds = 0.5f;
        [SerializeField] private float recoveryGraceSeconds = 0.5f;

        private IDeviceValidityProvider validityProvider;
        private float lossClock;
        private float recoveryClock;
        private bool considerLost;

        public void SetValidityProvider(IDeviceValidityProvider provider) => validityProvider = provider;

        private void Update()
        {
            bool valid = IsAnyValid();

            if (valid)
            {
                lossClock = 0f;
                if (considerLost)
                {
                    recoveryClock += Time.unscaledDeltaTime;
                    if (recoveryClock >= recoveryGraceSeconds)
                    {
                        considerLost = false;
                        recoveryClock = 0f;
                        InteractionPauseBus.RaiseResumed();
                    }
                }
            }
            else
            {
                recoveryClock = 0f;
                if (!considerLost)
                {
                    lossClock += Time.unscaledDeltaTime;
                    if (lossClock >= lossGraceSeconds)
                    {
                        considerLost = true;
                        lossClock = 0f;
                        InteractionPauseBus.RaisePaused();
                    }
                }
            }
        }

        private bool IsAnyValid()
        {
            if (validityProvider != null) return validityProvider.AnyValid;
            var list = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand, list);
            foreach (var device in list)
            {
                if (device.TryGetFeatureValue(CommonUsages.isTracked, out bool tracked) && tracked) return true;
            }
            return false;
        }
    }
}
