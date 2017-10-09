using UnityEngine;
using FrameSynthesis.VR;

namespace ScriptExample
{
    public class YesNo : MonoBehaviour
    {
        [SerializeField]
        VRGesture vrGesture;
        [SerializeField]
        ScriptEngine scriptEngine;
        [SerializeField]
        AudioSource gestureSound;

        void Start()
        {
            vrGesture.NodHandler += OnNod;
            vrGesture.HeadShakeHandler += OnHeadShake;
        }

        void OnNod()
        {
            if (scriptEngine.IsYesNoWaiting)
            {
                scriptEngine.AnswerYes();
                gestureSound.Play();
            }
        }

        void OnHeadShake()
        {
            if (scriptEngine.IsYesNoWaiting)
            {
                scriptEngine.AnswerNo();
                gestureSound.Play();
            }
        }
    }
}
