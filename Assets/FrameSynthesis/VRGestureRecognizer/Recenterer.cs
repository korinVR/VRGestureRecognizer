using UnityEngine;
using UnityEngine.XR;

namespace FrameSynthesis.VR
{
    public class Recenterer : MonoBehaviour
    {
        void Start()
        {
            XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                InputTracking.Recenter();
            }
        }
    }
}
