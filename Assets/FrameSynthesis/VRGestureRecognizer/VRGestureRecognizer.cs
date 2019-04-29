using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameSynthesis.VR
{
    public struct PoseSample
    {
        public float timestamp;
        public Quaternion orientation;
        public Vector3 eulerAngles;

        public PoseSample(float timestamp, Quaternion orientation)
        {
            this.timestamp = timestamp;
            this.orientation = orientation;

            eulerAngles = orientation.eulerAngles;
            eulerAngles.x = MyMath.WrapDegree(eulerAngles.x);
            eulerAngles.y = MyMath.WrapDegree(eulerAngles.y);
        }
    }

    public class VRGestureRecognizer : MonoBehaviour
    {
        public static VRGestureRecognizer Current { get; private set; }

        [SerializeField]
        float recognitionInterval = 0.5f;

        public event Action NodHandler;
        public event Action HeadshakeHandler;

        public LinkedList<PoseSample> PoseSamples { get; private set; }

        float prevGestureTime;

        void Awake()
        {
            Current = this;
        }

        void Start()
        {
            PoseSamples = new LinkedList<PoseSample>();
        }

        void Update()
        {
            var orientation = InputTracking.GetLocalRotation(XRNode.Head);

            // Record orientation
            PoseSamples.AddFirst(new PoseSample(Time.time, orientation));
            if (PoseSamples.Count >= 120)
            {
                PoseSamples.RemoveLast();
            }

            // Recognize gestures
            RecognizeNod();
            RecognizeHeadshake();
        }

        IEnumerable<PoseSample> Range(float startTime, float endTime)
        {
            return PoseSamples.Where(sample => (sample.timestamp < Time.time - startTime &&
                                            sample.timestamp >= Time.time - endTime));
        }

        void RecognizeNod()
        {
            try
            {
                var basePitch = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.x);
                var maxPitch = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.x);
                var pitch = PoseSamples.First().eulerAngles.x;

                if (maxPitch - basePitch > 10f &&
                    Mathf.Abs(pitch - basePitch) < 5f)
                {
                    if (prevGestureTime < Time.time - recognitionInterval)
                    {
                        prevGestureTime = Time.time;
                        NodHandler?.Invoke();
                    }
                }
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
                var baseYaw = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.y);
                var maxYaw = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.y);
                var minYaw = Range(0.01f, 0.2f).Min(sample => sample.eulerAngles.y);
                var yaw = PoseSamples.First().eulerAngles.y;

                if ((maxYaw - baseYaw > 10f || baseYaw - minYaw > 10f) &&
                    Mathf.Abs(yaw - baseYaw) < 5f)
                {
                    if (prevGestureTime < Time.time - recognitionInterval)
                    {
                        prevGestureTime = Time.time;
                        HeadshakeHandler?.Invoke();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
        }
    }
}

