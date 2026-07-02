using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SignTrainer.Core
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private float fadeSeconds = 0.5f;
        [SerializeField] private CanvasGroup fadeOverlay;

        public event Action<SceneName> SceneReady;
        private SceneName? currentContentScene;

        public void LoadSceneAsync(SceneName target) => StartCoroutine(LoadRoutine(target));

        private IEnumerator LoadRoutine(SceneName target)
        {
            GameLogger.Log($"SceneLoader → loading {target}");
            yield return FadeTo(1f);

            if (currentContentScene.HasValue)
            {
                var unload = SceneManager.UnloadSceneAsync(currentContentScene.Value.AsUnityName());
                if (unload != null) yield return unload;
            }

            var load = SceneManager.LoadSceneAsync(target.AsUnityName(), LoadSceneMode.Additive);
            if (load == null)
            {
                GameLogger.Error($"SceneLoader: could not load {target}.");
                yield break;
            }
            yield return load;

            var loadedScene = SceneManager.GetSceneByName(target.AsUnityName());
            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);
            }
            currentContentScene = target;

            yield return FadeTo(0f);
            SceneReady?.Invoke(target);
        }

        private IEnumerator FadeTo(float alpha)
        {
            if (fadeOverlay == null) yield break;
            float start = fadeOverlay.alpha;
            float t = 0f;
            while (t < fadeSeconds)
            {
                t += Time.deltaTime;
                fadeOverlay.alpha = Mathf.Lerp(start, alpha, Mathf.Clamp01(t / fadeSeconds));
                yield return null;
            }
            fadeOverlay.alpha = alpha;
        }
    }
}
