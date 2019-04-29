using UnityEngine;

namespace FrameSynthesis.VR.Example
{
    public class YesNoMessageGenerator : MonoBehaviour
    {
        [SerializeField]
        GameObject yesNoMessagePrefab;

        void Start()
        {
            VRGestureRecognizer.Current.NodHandler += OnNod;
            VRGestureRecognizer.Current.HeadshakeHandler += OnHeadshake;
        }

        void OnNod()
        {
            InstantiateYesNoMessage(true);
        }

        void OnHeadshake()
        {
            InstantiateYesNoMessage(false);
        }

        void InstantiateYesNoMessage(bool yesNo)
        {
            var go = Instantiate(yesNoMessagePrefab);
            go.GetComponent<YesNoMessage>().Initialize(yesNo);

            go.transform.SetParent(Camera.main.transform);
            go.transform.localPosition = new Vector3(0f, 0f, 10f);
            go.transform.localRotation = Quaternion.identity;
        }
    }
}
