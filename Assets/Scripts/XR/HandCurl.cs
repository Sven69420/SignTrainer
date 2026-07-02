using UnityEngine;
using UnityEngine.XR.Hands;

namespace SignTrainer.XR
{
    /// <summary>
    /// Computes per-finger extension from XR Hands joint poses. Pure geometry —
    /// <see cref="Straightness"/> is unit-testable without an XR device.
    /// </summary>
    public static class HandCurl
    {
        /// <summary>
        /// 1 = the three points are collinear (finger straight/extended),
        /// 0 = the segments fold fully back (finger curled).
        /// </summary>
        public static float Straightness(Vector3 a, Vector3 b, Vector3 c)
        {
            var v1 = b - a;
            var v2 = c - b;
            if (v1.sqrMagnitude < 1e-8f || v2.sqrMagnitude < 1e-8f) return 1f;
            float dot = Vector3.Dot(v1.normalized, v2.normalized);
            return Mathf.Clamp01((dot + 1f) * 0.5f);
        }

        public static bool TryFromHand(XRHand hand, out FingerExtensions ext)
        {
            ext = default;
            if (!hand.isTracked) return false;
            ext.thumb  = Finger(hand, XRHandJointID.ThumbProximal,  XRHandJointID.ThumbDistal,        XRHandJointID.ThumbTip);
            ext.index  = Finger(hand, XRHandJointID.IndexProximal,  XRHandJointID.IndexIntermediate,  XRHandJointID.IndexTip);
            ext.middle = Finger(hand, XRHandJointID.MiddleProximal, XRHandJointID.MiddleIntermediate, XRHandJointID.MiddleTip);
            ext.ring   = Finger(hand, XRHandJointID.RingProximal,   XRHandJointID.RingIntermediate,   XRHandJointID.RingTip);
            ext.pinky  = Finger(hand, XRHandJointID.LittleProximal, XRHandJointID.LittleIntermediate, XRHandJointID.LittleTip);
            ext.isTracked = true;
            return true;
        }

        static float Finger(XRHand hand, XRHandJointID a, XRHandJointID b, XRHandJointID c)
        {
            if (hand.GetJoint(a).TryGetPose(out var pa) &&
                hand.GetJoint(b).TryGetPose(out var pb) &&
                hand.GetJoint(c).TryGetPose(out var pc))
                return Straightness(pa.position, pb.position, pc.position);
            return 1f;
        }
    }
}
