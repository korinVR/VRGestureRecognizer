using UnityEngine;
using FrameSynthesis.VR;

namespace ScriptExample
{
    public class YesNoListener : MonoBehaviour
    {
        [SerializeField]
        VRGesture vrGesture;
        [SerializeField]
        ScriptEngine scriptEngine;

        void Start()
        {
            vrGesture.NodHandler += OnNod;
            vrGesture.HeadShakeHandler += OnHeadShake;
        }

        public void OnNod()
        {
            if (scriptEngine.IsYesNoWaiting)
            {
                scriptEngine.AnswerYes();
                GetComponent<AudioSource>().Play();
            }
        }

        public void OnHeadShake()
        {
            if (scriptEngine.IsYesNoWaiting)
            {
                scriptEngine.AnswerNo();
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
