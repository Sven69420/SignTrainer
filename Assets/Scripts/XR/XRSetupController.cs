using UnityEngine;
using SignTrainer.Core;

namespace SignTrainer.XR
{
    public class XRSetupController : MonoBehaviour
    {
        [SerializeField] private GameObject xrOriginRoot;
        [SerializeField] private TrackingMonitor trackingMonitor;

        public TrackingMonitor TrackingMonitor => trackingMonitor;
        public GameObject XROriginRoot => xrOriginRoot;

        private void Awake()
        {
            if (xrOriginRoot == null)
            {
                GameLogger.Error("XRSetupController: XR Origin reference missing.");
            }
        }
    }
}
