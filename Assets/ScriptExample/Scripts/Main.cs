using UnityEngine;
using FrameSynthesis.VR;

namespace ScriptExample
{
    public class Main : MonoBehaviour
    {
        [SerializeField] VRGestureRecognizer vrGestureRecognizer;
        [SerializeField] SimpleScriptEngine simpleScriptEngine;
        [SerializeField] AudioSource gestureSound;

        void Start()
        {
            vrGestureRecognizer.Nodded += OnNod;
            vrGestureRecognizer.HeadShaken += OnHeadshake;
        }

        void OnNod()
        {
            if (simpleScriptEngine.IsWaitingYesNo)
            {
                simpleScriptEngine.AnswerYes();
                gestureSound.Play();
            }
        }

        void OnHeadshake()
        {
            if (simpleScriptEngine.IsWaitingYesNo)
            {
                simpleScriptEngine.AnswerNo();
                gestureSound.Play();
            }
        }
    }
}
