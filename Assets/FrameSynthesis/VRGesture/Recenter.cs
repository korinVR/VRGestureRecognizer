using UnityEngine;
using UnityEngine.VR;

namespace FrameSynthesis.VR
{
    public class Recenter : MonoBehaviour
    {
        void Start()
        {
            UnityEngine.XR.XRDevice.SetTrackingSpaceType(UnityEngine.XR.TrackingSpaceType.Stationary);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                UnityEngine.XR.InputTracking.Recenter();
            }
        }
    }
}
