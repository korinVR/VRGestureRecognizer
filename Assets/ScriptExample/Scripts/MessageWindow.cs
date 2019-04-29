using UnityEngine;

namespace ScriptExample
{
    public class MessageWindow : MonoBehaviour
    {
        [SerializeField]
        ScriptEngine scriptEngine;

        [SerializeField]
        float interval = 0.05f;

        [SerializeField]
        TextMesh textMesh;
        [SerializeField]
        AudioSource audioSource;

        bool running;

        string message;
        float messageStartTime;

        int prevCursor;

        void Start()
        {
            scriptEngine.ShowMessageHandler += OnShowMessage;
        }

        void Update()
        {
            if (!running) return;

            var cursor = (int)((Time.time - messageStartTime) / interval);
            if (cursor >= message.Length)
            {
                running = false;
                scriptEngine.NextCommand();
                return;
            }

            textMesh.text = message.Substring(0, cursor + 1);

            if (prevCursor != cursor)
            {
                prevCursor = cursor;
                var letter = message[cursor];
                if (letter != 32 && letter != 10)
                {
                    audioSource.Play();
                }
            }
        }

        void OnShowMessage(string message)
        {
            this.message = message;
            messageStartTime = Time.time;
            running = true;
            prevCursor = -1;
        }
    }
}
