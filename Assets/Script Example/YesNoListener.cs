using UnityEngine;

public class YesNoListener : MonoBehaviour
{
    public string yesLabel;
    public string noLabel;

    public void TriggerYes()
    {
        GameObject.Find("Script Engine").SendMessage("GoTo", yesLabel);
        audio.Play();
        Destroy(this);
    }
    
    public void TriggerNo()
    {
        GameObject.Find("Script Engine").SendMessage("GoTo", noLabel);
        audio.Play();
        Destroy(this);
    }
}
