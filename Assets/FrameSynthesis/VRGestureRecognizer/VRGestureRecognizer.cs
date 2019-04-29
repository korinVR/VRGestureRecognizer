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

        float waitTime;

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
            var q = InputTracking.GetLocalRotation(XRNode.Head);

            // Record orientation
            PoseSamples.AddFirst(new PoseSample(Time.time, q));
            if (PoseSamples.Count >= 120)
            {
                PoseSamples.RemoveLast();
            }

            // Recognize gestures
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                return;
            }

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
                var basePos = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.x);
                var xMax = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.x);
                var current = PoseSamples.First().eulerAngles.x;

                if (xMax - basePos > 10f &&
                    Mathf.Abs(current - basePos) < 5f)
                {
                    NodHandler?.Invoke();
                    waitTime = recognitionInterval;
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
                var basePos = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.y);
                var yMax = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.y);
                var yMin = Range(0.01f, 0.2f).Min(sample => sample.eulerAngles.y);
                var current = PoseSamples.First().eulerAngles.y;

                if ((yMax - basePos > 10f || basePos - yMin > 10f) &&
                    Mathf.Abs(current - basePos) < 5f)
                {
                    HeadshakeHandler?.Invoke();
                    waitTime = recognitionInterval;
                }
            }
            catch (InvalidOperationException)
            {
                // Range contains no entry
            }
        }
    }
}

