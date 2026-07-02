using UnityEngine;

namespace SignTrainer.Data
{
    /// <summary>Desired state of one finger in a target hand shape.</summary>
    public enum FingerTarget { Curled, Extended, Any }

    /// <summary>
    /// Rule-based target hand shape for approximate fingerspelling validation
    /// (per spec D8: key-point checkpoint, no ML classifier). One asset per letter.
    /// Finger order: thumb, index, middle, ring, pinky.
    /// </summary>
    [CreateAssetMenu(menuName = "SignTrainer/Hand Shape", fileName = "HS_NewShape")]
    public class HandShapeData : ScriptableObject
    {
        public string label = "A";
        public FingerTarget thumb = FingerTarget.Any;
        public FingerTarget index = FingerTarget.Curled;
        public FingerTarget middle = FingerTarget.Curled;
        public FingerTarget ring = FingerTarget.Curled;
        public FingerTarget pinky = FingerTarget.Curled;

        [Tooltip("Higher = more forgiving matching."), Range(0.1f, 0.5f)]
        public float tolerance = 0.3f;

        /// <summary>Finger by index 0..4 = thumb, index, middle, ring, pinky.</summary>
        public FingerTarget Get(int finger) => finger switch
        {
            0 => thumb,
            1 => index,
            2 => middle,
            3 => ring,
            _ => pinky,
        };
    }
}
