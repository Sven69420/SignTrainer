using UnityEngine;
using SignTrainer.Data;

namespace SignTrainer.XR
{
    /// <summary>
    /// Scores a live hand against a target <see cref="HandShapeData"/> using per-finger
    /// extension. Pure and unit-testable. Reuses <see cref="PoseVerdict"/>.
    /// </summary>
    public static class HandShapeMatcher
    {
        // Gates: at/above ExtHi a finger reads fully extended; at/below CurlLo fully curled.
        const float ExtLo = 0.4f, ExtHi = 0.8f;
        const float CurlLo = 0.2f, CurlHi = 0.6f;

        /// <summary>Returns 0..1; 1 = every scored finger sits clearly in its target region.</summary>
        public static float Score(FingerExtensions live, HandShapeData target)
        {
            if (target == null || !live.isTracked) return 0f;

            float total = 0f;
            int scored = 0;

            for (int f = 0; f < 5; f++)
            {
                var t = target.Get(f);
                if (t == FingerTarget.Any) continue;
                float e = live.Get(f);
                total += t == FingerTarget.Extended
                    ? Mathf.Clamp01((e - ExtLo) / (ExtHi - ExtLo))
                    : Mathf.Clamp01((CurlHi - e) / (CurlHi - CurlLo));
                scored++;
            }

            return scored == 0 ? 1f : total / scored;
        }

        public static PoseVerdict Verdict(float score, float correct = 0.8f, float almost = 0.55f)
            => score >= correct ? PoseVerdict.Correct
             : score >= almost ? PoseVerdict.AlmostThere
             : PoseVerdict.TryAgain;
    }
}
