using NUnit.Framework;
using UnityEngine;
using SignTrainer.Data;
using SignTrainer.XR;

namespace SignTrainer.Tests
{
    public class HandShapeLogicTests
    {
        [Test]
        public void Straightness_Collinear_IsOne()
        {
            float s = HandCurl.Straightness(Vector3.zero, new Vector3(0, 0, 1), new Vector3(0, 0, 2));
            Assert.AreEqual(1f, s, 0.001f);
        }

        [Test]
        public void Straightness_RightAngle_IsHalf()
        {
            float s = HandCurl.Straightness(Vector3.zero, new Vector3(0, 0, 1), new Vector3(0, 1, 1));
            Assert.AreEqual(0.5f, s, 0.001f);
        }

        [Test]
        public void Straightness_FoldedBack_IsZero()
        {
            float s = HandCurl.Straightness(Vector3.zero, new Vector3(0, 0, 1), Vector3.zero);
            Assert.AreEqual(0f, s, 0.001f);
        }

        [Test]
        public void Score_PerfectExtendedHand_IsOne()
        {
            var target = AllExtended();
            var live = new FingerExtensions { isTracked = true, thumb = 1, index = 1, middle = 1, ring = 1, pinky = 1 };
            Assert.AreEqual(1f, HandShapeMatcher.Score(live, target), 0.001f);
            Assert.AreEqual(PoseVerdict.Correct, HandShapeMatcher.Verdict(HandShapeMatcher.Score(live, target)));
        }

        [Test]
        public void Score_CurledHandAgainstExtendedTarget_FailsVerdict()
        {
            var target = AllExtended();
            var live = new FingerExtensions { isTracked = true, thumb = 0, index = 0, middle = 0, ring = 0, pinky = 0 };
            Assert.AreEqual(PoseVerdict.TryAgain, HandShapeMatcher.Verdict(HandShapeMatcher.Score(live, target)));
        }

        [Test]
        public void Score_LetterB_MatchesThumbCurledFingersExtended()
        {
            var b = ScriptableObject.CreateInstance<HandShapeData>();
            b.thumb = FingerTarget.Curled;
            b.index = b.middle = b.ring = b.pinky = FingerTarget.Extended;
            var live = new FingerExtensions { isTracked = true, thumb = 0, index = 1, middle = 1, ring = 1, pinky = 1 };
            Assert.AreEqual(1f, HandShapeMatcher.Score(live, b), 0.001f);
        }

        [Test]
        public void Score_AnyFingersIgnored()
        {
            var any = ScriptableObject.CreateInstance<HandShapeData>();
            any.thumb = any.index = any.middle = any.ring = any.pinky = FingerTarget.Any;
            var live = new FingerExtensions { isTracked = true, thumb = 0.5f, index = 0.5f, middle = 0.5f, ring = 0.5f, pinky = 0.5f };
            Assert.AreEqual(1f, HandShapeMatcher.Score(live, any), 0.001f);
        }

        [Test]
        public void Score_UntrackedHand_IsZero()
        {
            var live = new FingerExtensions { isTracked = false };
            Assert.AreEqual(0f, HandShapeMatcher.Score(live, AllExtended()), 0.001f);
        }

        static HandShapeData AllExtended()
        {
            var d = ScriptableObject.CreateInstance<HandShapeData>();
            d.thumb = d.index = d.middle = d.ring = d.pinky = FingerTarget.Extended;
            return d;
        }
    }
}
