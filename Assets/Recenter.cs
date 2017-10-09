using UnityEngine;
using UnityEngine.VR;

public class Recenter : MonoBehaviour
{
    void Start()
    {
        VRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            InputTracking.Recenter();
        }
    }
}