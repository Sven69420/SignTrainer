#if UNITY_EDITOR
// Editor-only keyboard tester — remove before shipping.
// Keys: H=Hello  T=ThankYou  P=Please  Y=Yes  N=No  S=Sorry  L=Help
//       G=Good   B=GoodBye   F=Friend  X=Stop  W=Wait
using UnityEngine;
using SignTrainer.Data;
using SignTrainer.Tutorial;

namespace SignTrainer.Scenario
{
    public class KeyboardTestDriver : MonoBehaviour
    {
        [Header("Drag from Assets/ScriptableObjects/Signs")]
        public SignData hello;
        public SignData thankYou;
        public SignData please;
        public SignData yes;
        public SignData no;
        public SignData sorry;
        public SignData help;
        public SignData good;
        public SignData goodBye;
        public SignData friend;
        public SignData stop;
        public SignData wait;

        SignDemoPlayer _player;

        void Awake() => _player = FindObjectOfType<SignDemoPlayer>();

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.H)) Play(hello);
            if (Input.GetKeyDown(KeyCode.T)) Play(thankYou);
            if (Input.GetKeyDown(KeyCode.P)) Play(please);
            if (Input.GetKeyDown(KeyCode.Y)) Play(yes);
            if (Input.GetKeyDown(KeyCode.N)) Play(no);
            if (Input.GetKeyDown(KeyCode.S)) Play(sorry);
            if (Input.GetKeyDown(KeyCode.L)) Play(help);
            if (Input.GetKeyDown(KeyCode.G)) Play(good);
            if (Input.GetKeyDown(KeyCode.B)) Play(goodBye);
            if (Input.GetKeyDown(KeyCode.F)) Play(friend);
            if (Input.GetKeyDown(KeyCode.X)) Play(stop);
            if (Input.GetKeyDown(KeyCode.W)) Play(wait);
        }

        void Play(SignData sign)
        {
            if (sign == null) { Debug.LogWarning("[KeyboardTest] SignData slot not assigned in Inspector."); return; }
            if (_player == null) { Debug.LogWarning("[KeyboardTest] No SignDemoPlayer found in scene."); return; }
            _player.Play(sign);
        }
    }
}
#endif
