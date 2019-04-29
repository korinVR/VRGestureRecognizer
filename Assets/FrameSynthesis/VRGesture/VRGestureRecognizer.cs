using UnityEngine;
using UnityEngine.XR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrameSynthesis.VR
{
    struct Sample
    {
        public float timestamp;
        public Quaternion orientation;
        public Vector3 eulerAngles;

        public Sample(float timestamp, Quaternion orientation)
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

        LinkedList<Sample> samples = new LinkedList<Sample>();
        float waitTime;

        void Awake()
        {
            Current = this;
        }

        void Update()
        {
            var q = InputTracking.GetLocalRotation(XRNode.Head);

            // Record orientation
            samples.AddFirst(new Sample(Time.time, q));
            if (samples.Count >= 120)
            {
                samples.RemoveLast();
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

        public void GetGraphEntries(out float[] timestamps, out Quaternion[] orientations)
        {
            var size = samples.Count;
            timestamps = new float[size];
            orientations = new Quaternion[size];

            var index = 0;
            foreach (var sample in samples)
            {
                timestamps[index] = sample.timestamp;
                orientations[index] = sample.orientation;
                index++;
            }
        }

        IEnumerable<Sample> Range(float startTime, float endTime)
        {
            return samples.Where(sample => (sample.timestamp < Time.time - startTime &&
                                            sample.timestamp >= Time.time - endTime));
        }

        void RecognizeNod()
        {
            try
            {
                var basePos = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.x);
                var xMax = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.x);
                var current = samples.First().eulerAngles.x;

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
                var current = samples.First().eulerAngles.y;

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

