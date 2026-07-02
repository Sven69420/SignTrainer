using System.Collections.Generic;
using UnityEngine;
using SignTrainer.Core;
using SignTrainer.Core.Buses;
using SignTrainer.Data;
using SignTrainer.UI;

namespace SignTrainer.Tutorial
{
    public enum TutorialState { Idle, ShowingList, SignSelected, Demoing, DemoEnded, Attempting, Feedback }

    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private List<SignData> mvpSignList = new List<SignData>();
        [SerializeField] private SignDemoPlayer demoPlayer;
        [SerializeField] private PromptPanelController promptPanel;

        public TutorialState State { get; private set; } = TutorialState.Idle;
        public SignData CurrentSign { get; private set; }

        private void Start()
        {
            if (demoPlayer != null) demoPlayer.DemoCompleted += OnDemoCompleted;
            SetState(TutorialState.ShowingList);
        }

        private void OnDestroy()
        {
            if (demoPlayer != null) demoPlayer.DemoCompleted -= OnDemoCompleted;
        }

        public IReadOnlyList<SignData> GetSignList() => mvpSignList;

        public void SelectSign(SignData sign)
        {
            if (sign == null) return;
            CurrentSign = sign;
            SetState(TutorialState.SignSelected);
            if (promptPanel != null) promptPanel.Show(sign.displayName, sign.meaning);
            PlayDemo();
        }

        public void PlayDemo()
        {
            if (CurrentSign == null || demoPlayer == null) return;
            SetState(TutorialState.Demoing);
            demoPlayer.Play(CurrentSign);
        }

        public void Replay() => PlayDemo();

        public void ToggleSlowMo(bool enabled) => demoPlayer?.SetSlowMo(enabled);

        public void MarkAttempted()
        {
            if (CurrentSign != null) SignCompletionBus.Raise(CurrentSign.signId);
            SetState(TutorialState.ShowingList);
        }

        private void OnDemoCompleted() => SetState(TutorialState.DemoEnded);

        private void SetState(TutorialState next)
        {
            GameLogger.Log($"TutorialManager: {State} → {next}");
            State = next;
        }
    }
}
