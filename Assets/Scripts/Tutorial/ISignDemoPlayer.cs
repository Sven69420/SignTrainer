using System;
using SignTrainer.Data;

namespace SignTrainer.Tutorial
{
    public interface ISignDemoPlayer
    {
        event Action DemoCompleted;
        void Play(SignData sign);
        void Replay();
        void SetSlowMo(bool enabled);
        void Pause();
        void Resume();
    }
}
