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
        AudioSource gestureSoundYes;
        [SerializeField]
        AudioSource gestureSoundNo;

		void Start()
        {
            vrGesture.NodHandler += OnNod;
            vrGesture.HeadshakeHandler += OnHeadshake;
        }

        void OnNod()
        {
            if (scriptEngine.IsYesNoWaiting)
            {
                scriptEngine.AnswerYes();
                gestureSoundYes.Play();
            }
        }

        void OnHeadshake()
        {
            if (scriptEngine.IsYesNoWaiting)
            {
                scriptEngine.AnswerNo();
                gestureSoundNo.Play();
            }
        }
    }
}
