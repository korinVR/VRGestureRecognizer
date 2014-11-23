using UnityEngine;

public class YesNo : MonoBehaviour
{
    public Transform messagePrefab;

    public void TriggerYes()
    {
        SpawnMessage("Yes!");
    }

    public void TriggerNo()
    {
        SpawnMessage("No!");
    }

    void SpawnMessage(string text)
    {
        Transform message = (Transform) Instantiate(messagePrefab);
        message.parent = GameObject.Find("CameraRight").transform;
        message.transform.localPosition = new Vector3(0f, 0f, 10);
        message.transform.localRotation = Quaternion.identity;
        message.SendMessage("SetText", text);
    }

}

