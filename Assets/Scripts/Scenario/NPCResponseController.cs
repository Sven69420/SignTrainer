using System;
using UnityEngine;

namespace SignTrainer.Scenario
{
    public class NPCResponseController : MonoBehaviour
    {
        [SerializeField] private Animator npcAnimator;

        public event Action ResponsePlayed;

        public void PlayResponse(string trigger)
        {
            if (npcAnimator != null && !string.IsNullOrEmpty(trigger))
                npcAnimator.SetTrigger(trigger);
            ResponsePlayed?.Invoke();
        }
    }
}
