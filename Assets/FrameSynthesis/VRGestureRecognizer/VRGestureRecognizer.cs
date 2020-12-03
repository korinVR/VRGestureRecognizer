using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameSynthesis.VR
{
    public struct PoseSample
    {
        public readonly float Timestamp;
        public Quaternion Orientation;
        public Vector3 EulerAngles;

        public PoseSample(float timestamp, Quaternion orientation)
        {
            Timestamp = timestamp;
            Orientation = orientation;

            EulerAngles = orientation.eulerAngles;
            EulerAngles.x = MathHelper.WrapDegree(EulerAngles.x);
            EulerAngles.y = MathHelper.WrapDegree(EulerAngles.y);
        }
    }

    public class VRGestureRecognizer : MonoBehaviour
    {
        [SerializeField] float recognitionInterval = 0.5f;

        public event Action Nodded;
        public event Action HeadShaken;

        public readonly Queue<PoseSample> PoseSamples = new Queue<PoseSample>();

        float prevGestureTime;

        void Update()
        {
            var orientation = InputTracking.GetLocalRotation(XRNode.Head);

            // Record orientation
            PoseSamples.Enqueue(new PoseSample(Time.time, orientation));
            if (PoseSamples.Count >= 120)
            {
                PoseSamples.Dequeue();
            }

            // Recognize gestures
            RecognizeNod();
            RecognizeHeadshake();
        }

        IEnumerable<PoseSample> Range(float startTime, float endTime) =>
            PoseSamples.Where(sample => 
                sample.Timestamp < Time.time - startTime && 
                sample.Timestamp >= Time.time - endTime);

        void RecognizeNod()
        {
            try
            {
                var averagePitch = Range(0.2f, 0.4f).Average(sample => sample.EulerAngles.x);
                var maxPitch = Range(0.01f, 0.2f).Max(sample => sample.EulerAngles.x);
                var pitch = PoseSamples.First().EulerAngles.x;

                if (!(maxPitch - averagePitch > 10f) || !(Mathf.Abs(pitch - averagePitch) < 5f)) return;
                if (!(prevGestureTime < Time.time - recognitionInterval)) return;
                
                prevGestureTime = Time.time;
                Nodded?.Invoke();
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
        }

        void RecognizeHeadshake()
        {
            try
            {
                var averageYaw = Range(0.2f, 0.4f).Average(sample => sample.EulerAngles.y);
                var maxYaw = Range(0.01f, 0.2f).Max(sample => sample.EulerAngles.y);
                var minYaw = Range(0.01f, 0.2f).Min(sample => sample.EulerAngles.y);
                var yaw = PoseSamples.First().EulerAngles.y;

                if ((!(maxYaw - averageYaw > 10f) && !(averageYaw - minYaw > 10f)) || !(Mathf.Abs(yaw - averageYaw) < 5f)) return;
                if (!(prevGestureTime < Time.time - recognitionInterval)) return;
                
                prevGestureTime = Time.time;
                HeadShaken?.Invoke();
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
        }
    }
}
