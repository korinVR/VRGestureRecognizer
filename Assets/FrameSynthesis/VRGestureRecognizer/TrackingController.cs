using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace FrameSynthesis.VR
{
    public class TrackingController : MonoBehaviour
    {
        readonly List<XRInputSubsystem> subsystems = new List<XRInputSubsystem>();
        
        void Start()
        {
            SubsystemManager.GetInstances(subsystems);
            subsystems.ForEach(subsystem => subsystem.TrySetTrackingOriginMode(TrackingOriginModeFlags.Device));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                subsystems.ForEach(subsystem => subsystem.TryRecenter());
            }
        }
    }
}
