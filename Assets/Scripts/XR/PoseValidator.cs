using UnityEngine;
using SignTrainer.Data;

namespace SignTrainer.XR
{
    public enum PoseVerdict { Correct, AlmostThere, TryAgain }

    public class PoseValidator : MonoBehaviour
    {
        [SerializeField] private Transform torsoAnchor;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField, Range(0.5f, 1f)] private float correctThreshold = 0.8f;
        [SerializeField, Range(0.2f, 0.9f)] private float almostThreshold = 0.5f;

        public PoseVerdict Evaluate(PoseReference reference, out float score)
        {
            if (reference == null || torsoAnchor == null)
            {
                score = 1f;
                return PoseVerdict.Correct;
            }

            float leftPosErr = LocalDistance(leftHand, reference.leftHandLocalPos);
            float rightPosErr = LocalDistance(rightHand, reference.rightHandLocalPos);
            float leftRotErr = LocalAngle(leftHand, reference.leftHandLocalEulers);
            float rightRotErr = LocalAngle(rightHand, reference.rightHandLocalEulers);

            float posErr = (leftPosErr + rightPosErr) * 0.5f;
            float rotErr = (leftRotErr + rightRotErr) * 0.5f;
            float posScore = 1f - Mathf.Clamp01(posErr / reference.positionTolerance);
            float rotScore = 1f - Mathf.Clamp01(rotErr / reference.rotationToleranceDeg);

            score = posScore * 0.6f + rotScore * 0.4f;
            if (score >= correctThreshold) return PoseVerdict.Correct;
            if (score >= almostThreshold) return PoseVerdict.AlmostThere;
            return PoseVerdict.TryAgain;
        }

        private float LocalDistance(Transform hand, Vector3 targetLocal)
        {
            if (hand == null) return float.MaxValue;
            var actual = torsoAnchor.InverseTransformPoint(hand.position);
            return Vector3.Distance(actual, targetLocal);
        }

        private float LocalAngle(Transform hand, Vector3 targetLocalEulers)
        {
            if (hand == null) return 180f;
            var actualLocal = Quaternion.Inverse(torsoAnchor.rotation) * hand.rotation;
            var target = Quaternion.Euler(targetLocalEulers);
            return Quaternion.Angle(actualLocal, target);
        }
    }
}
