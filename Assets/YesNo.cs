using UnityEngine;
using FrameSynthesis.VR;

public class YesNo : MonoBehaviour
{
    [SerializeField]
    VRGesture vrGesture;

    [SerializeField]
    GameObject messagePrefab;

    void Start()
    {
        vrGesture.NodHandler += OnNod;
        vrGesture.HeadShakeHandler += OnHeadShake;
    }

    public void OnNod()
    {
        SpawnMessage("Yes!");
    }

    public void OnHeadShake()
    {
        SpawnMessage("No!");
    }

    void SpawnMessage(string text)
    {
        var message = Instantiate(messagePrefab);
        message.transform.SetParent(Camera.main.transform);
        message.transform.localPosition = new Vector3(0f, 0f, 10f);
        message.transform.localRotation = Quaternion.identity;
        message.SendMessage("SetText", text);
    }
}
