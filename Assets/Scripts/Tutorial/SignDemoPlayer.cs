using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;
using SignTrainer.Core;
using SignTrainer.Core.Buses;
using SignTrainer.Data;

namespace SignTrainer.Tutorial
{
    [RequireComponent(typeof(Animator))]
    public class SignDemoPlayer : MonoBehaviour, ISignDemoPlayer
    {
        public event Action DemoCompleted;

        private Animator animator;
        private PlayableGraph graph;
        private AnimationClipPlayable clipPlayable;
        private AnimationPlayableOutput output;
        private SignData currentSign;
        private bool slowMo;
        private bool paused;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            graph = PlayableGraph.Create("SignDemoGraph");
            output = AnimationPlayableOutput.Create(graph, "SignAnimation", animator);
        }

        private void OnEnable()
        {
            InteractionPauseBus.Paused += Pause;
            InteractionPauseBus.Resumed += Resume;
        }

        private void OnDisable()
        {
            InteractionPauseBus.Paused -= Pause;
            InteractionPauseBus.Resumed -= Resume;
        }

        private void OnDestroy()
        {
            if (graph.IsValid()) graph.Destroy();
        }

        public void Play(SignData sign)
        {
            if (sign == null || sign.demoAnimation == null)
            {
                GameLogger.Warn($"SignDemoPlayer: no clip for sign {sign?.signId}");
                return;
            }
            currentSign = sign;
            if (clipPlayable.IsValid()) clipPlayable.Destroy();
            clipPlayable = AnimationClipPlayable.Create(graph, sign.demoAnimation);
            output.SetSourcePlayable(clipPlayable);
            clipPlayable.SetTime(0);
            clipPlayable.SetSpeed(slowMo ? sign.slowMoSpeed : 1f);
            graph.Play();
        }

        public void Replay()
        {
            if (currentSign == null) return;
            Play(currentSign);
        }

        public void SetSlowMo(bool enabled)
        {
            slowMo = enabled;
            if (clipPlayable.IsValid() && currentSign != null)
                clipPlayable.SetSpeed(enabled ? currentSign.slowMoSpeed : 1f);
        }

        public void Pause()
        {
            if (graph.IsValid() && !paused) { graph.Stop(); paused = true; }
        }

        public void Resume()
        {
            if (graph.IsValid() && paused) { graph.Play(); paused = false; }
        }

        private void Update()
        {
            if (!clipPlayable.IsValid() || currentSign?.demoAnimation == null) return;
            if (clipPlayable.GetTime() >= currentSign.demoAnimation.length && !paused)
            {
                DemoCompleted?.Invoke();
            }
        }
    }
}
