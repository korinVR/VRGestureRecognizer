using UnityEngine;
using FrameSynthesis.VR;

namespace ScriptExample
{
    public class Main : MonoBehaviour
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
            vrGesture.HeadshakeHandler += OnHeadshake;
        }

        void OnNod()
        {
            if (scriptEngine.IsWaitingYesNo)
            {
                scriptEngine.AnswerYes();
                gestureSound.Play();
            }
        }

        void OnHeadshake()
        {
            if (scriptEngine.IsWaitingYesNo)
            {
                scriptEngine.AnswerNo();
                gestureSound.Play();
            }
        }
    }
}
