using UnityEngine;
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
            eulerAngles.x = MyMath.wrapAngle(eulerAngles.x);
            eulerAngles.y = MyMath.wrapAngle(eulerAngles.y);
        }
    }

    public class RiftGesture : MonoBehaviour
    {
        public int sensor = 0;

        LinkedList<Sample> samples;
        float waitTime = 0f;
        const float detectInterval = 0.5f;

        public RiftGesture()
        {
            samples = new LinkedList<Sample>();
        }

        public void Update()
        {
            // Recode orientation
			Vector3 p = Vector3.zero;
            Quaternion q = Quaternion.identity;
			OVRDevice.GetCameraPositionOrientation(ref p, ref q);

            samples.AddFirst(new Sample(Time.time, q));
            if (samples.Count >= 60) {
                samples.RemoveLast();
            }

            // Detect gestures
            if (waitTime > 0) {
                waitTime -= Time.deltaTime;
            } else {
                DetectNod();
                DetectHeadshake();
            }
        }

        public void GetGraphEntries(out float[] timestamps, out Quaternion[] orientations)
        {
            int size = samples.Count;
            timestamps = new float[size];
            orientations = new Quaternion[size];

            int index = 0;
            foreach (var sample in samples) {
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

        void DetectNod()
        {
            try {
                float basePos = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.x);
                float xMax = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.x);
                float current = samples.First().eulerAngles.x;

                if (xMax - basePos > 10f &&
                    Mathf.Abs(current - basePos) < 5f) {
                    SendMessage("TriggerYes", SendMessageOptions.DontRequireReceiver);
                    waitTime = detectInterval;
                }
            } catch (InvalidOperationException) {
                // Range contains no entry
            }
        }

        void DetectHeadshake()
        {
            try {
                float basePos = Range(0.2f, 0.4f).Average(sample => sample.eulerAngles.y);
                float yMax = Range(0.01f, 0.2f).Max(sample => sample.eulerAngles.y);
                float yMin = Range(0.01f, 0.2f).Min(sample => sample.eulerAngles.y);
                float current = samples.First().eulerAngles.y;

                if ((yMax - basePos > 10f || basePos - yMin > 10f) &&
                    Mathf.Abs(current - basePos) < 5f) {
                    SendMessage("TriggerNo", SendMessageOptions.DontRequireReceiver);
                    waitTime = detectInterval;
                }
            } catch (InvalidOperationException) {
                // Range contains no entry
            }
        }
    }
}

