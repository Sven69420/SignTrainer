using UnityEngine;

namespace SignTrainer.XR
{
    public class SceneSpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform xrOriginRoot;
        [SerializeField] private Transform spawnPoint;

        private void Start()
        {
            if (xrOriginRoot == null)
            {
                var xrOrigin = FindFirstObjectByType<Unity.XR.CoreUtils.XROrigin>();
                if (xrOrigin != null) xrOriginRoot = xrOrigin.transform;
            }

            if (spawnPoint == null)
                spawnPoint = transform;

            if (xrOriginRoot == null || spawnPoint == null)
                return;

            xrOriginRoot.SetPositionAndRotation(
                spawnPoint.position,
                spawnPoint.rotation
            );
        }
    }
}