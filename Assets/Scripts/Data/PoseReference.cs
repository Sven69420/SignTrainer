using UnityEngine;

namespace SignTrainer.Data
{
    [CreateAssetMenu(menuName = "SignTrainer/Pose Reference", fileName = "PoseRef_NewSign")]
    public class PoseReference : ScriptableObject
    {
        public Vector3 leftHandLocalPos;
        public Vector3 rightHandLocalPos;
        public Vector3 leftHandLocalEulers;
        public Vector3 rightHandLocalEulers;

        [Range(0.05f, 0.5f)] public float positionTolerance = 0.15f;
        [Range(10f, 90f)] public float rotationToleranceDeg = 30f;
    }
}
