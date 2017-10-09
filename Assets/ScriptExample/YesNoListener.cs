using UnityEngine;

public class YesNoListener : MonoBehaviour
{
    public string yesLabel;
    public string noLabel;

    public void TriggerYes()
    {
        GameObject.Find("Script Engine").SendMessage("GoTo", yesLabel);
        GetComponent<AudioSource>().Play();
        Destroy(this);
    }

    public void TriggerNo()
    {
        GameObject.Find("Script Engine").SendMessage("GoTo", noLabel);
        GetComponent<AudioSource>().Play();
        Destroy(this);
    }
}
