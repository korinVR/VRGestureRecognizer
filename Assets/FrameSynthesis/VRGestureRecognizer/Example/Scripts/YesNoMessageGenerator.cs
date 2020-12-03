using UnityEngine;

namespace FrameSynthesis.VR.Example
{
    public class YesNoMessageGenerator : MonoBehaviour
    {
        [SerializeField] VRGestureRecognizer vrGestureRecognizer;
        [SerializeField] GameObject yesNoMessagePrefab;

        void Start()
        {
            vrGestureRecognizer.Nodded += OnNodded;
            vrGestureRecognizer.HeadShaken += OnHeadShaken;
        }

        void OnNodded()
        {
            InstantiateYesNoMessage(true);
        }

        void OnHeadShaken()
        {
            InstantiateYesNoMessage(false);
        }

        void InstantiateYesNoMessage(bool yesNo)
        {
            var go = Instantiate(yesNoMessagePrefab, Camera.main.transform, true);
            go.GetComponent<YesNoMessage>().Initialize(yesNo);

            go.transform.localPosition = new Vector3(0f, 0f, 10f);
            go.transform.localRotation = Quaternion.identity;
        }
    }
}
