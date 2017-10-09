using UnityEngine;

namespace FrameSynthesis.VR.Example
{
    [RequireComponent(typeof(TextMesh))]
    public class Message : MonoBehaviour
    {
        [SerializeField]
        float lifetime = 1f;
        [SerializeField]
        Vector3 velocity;

        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        public void SetText(string text)
        {
            GetComponent<TextMesh>().text = text;
        }

        void Update()
        {
            transform.Translate(velocity * Time.deltaTime);
        }
    }
}
