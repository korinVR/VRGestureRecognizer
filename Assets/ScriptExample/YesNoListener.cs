using UnityEngine;
using FrameSynthesis.VR;

public class YesNoListener : MonoBehaviour
{
    [SerializeField]
    VRGesture vrGesture;
    [SerializeField]
    ScriptEngine scriptEngine;

    void Start()
    {
        vrGesture.NodHandler += TriggerYes;
        vrGesture.HeadShakeHandler += TriggerNo;
    }

    public void TriggerYes()
    {
        if (scriptEngine.IsYesNoWaiting)
        {
            scriptEngine.AnswerYes();
            GetComponent<AudioSource>().Play();
        }
    }

    public void TriggerNo()
    {
        if (scriptEngine.IsYesNoWaiting)
        {
            scriptEngine.AnswerNo();
            GetComponent<AudioSource>().Play();
        }
    }
}
