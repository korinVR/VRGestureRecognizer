using UnityEngine;
using UnityEngine.VR;
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
            eulerAngles.x = MyMath.WrapAngle(eulerAngles.x);
            eulerAngles.y = MyMath.WrapAngle(eulerAngles.y);
        }
    }

    public class VRGesture : MonoBehaviour
    {
        [SerializeField]
        float recognitionInterval = 0.5f;

        public event Action NodHandler;
        public event Action HeadshakeHandler;

        LinkedList<Sample> samples = new LinkedList<Sample>();
        float waitTime = 0f;

        void Update()
        {
            // Record orientation
            Quaternion q = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head);

            samples.AddFirst(new Sample(Time.time, q));
            if (samples.Count >= 120)
            {
                samples.RemoveLast();
            }

            // Recognize gestures
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                RecognizeNod();
                RecognizeHeadshake();
            }
        }

        public void GetGraphEntries(out float[] timestamps, out Quaternion[] orientations)
        {
            int size = samples.Count;
            timestamps = new float[size];
            orientations = new Quaternion[size];

            int index = 0;
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
                float basePos = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.x);
                float xMax = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.x);
                float current = samples.First().eulerAngles.x;

                if (xMax - basePos > 10f &&
                    Mathf.Abs(current - basePos) < 5f)
                {
                    if (NodHandler != null) { NodHandler.Invoke(); }
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
                float basePos = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.y);
                float yMax = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.y);
                float yMin = Range(0.01f, 0.2f).Min(sample => sample.eulerAngles.y);
                float current = samples.First().eulerAngles.y;

                if ((yMax - basePos > 10f || basePos - yMin > 10f) &&
                    Mathf.Abs(current - basePos) < 5f)
                {
                    if (HeadshakeHandler != null) { HeadshakeHandler.Invoke(); }
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

