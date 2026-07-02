using System.Collections.Generic;
using UnityEngine;

namespace SignTrainer.Data
{
    [CreateAssetMenu(menuName = "SignTrainer/Sign", fileName = "SGN-XXX_NewSign")]
    public class SignData : ScriptableObject
    {
        [Header("Identity")]
        public string signId;
        public string displayName;
        [TextArea(2, 4)] public string meaning;

        [Header("Classification")]
        public SignCategory category;
        public int module;
        public bool isMvp;

        [Header("Playback")]
        public AnimationClip demoAnimation;
        public List<SignData> phraseSequence = new List<SignData>();
        [Range(0.2f, 1f)] public float slowMoSpeed = 0.5f;

        [Header("Validation (optional)")]
        public PoseReference optionalPoseReference;

        [Header("Narration (optional)")]
        public AudioClip voiceOver;

        public bool IsPhrase => phraseSequence != null && phraseSequence.Count > 0;
    }
}
