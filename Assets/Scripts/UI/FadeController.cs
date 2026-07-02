using UnityEngine;

namespace SignTrainer.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeController : MonoBehaviour
    {
        private CanvasGroup group;

        private void Awake() => group = GetComponent<CanvasGroup>();

        public void SetAlpha(float alpha)
        {
            if (group != null) group.alpha = Mathf.Clamp01(alpha);
        }
    }
}
