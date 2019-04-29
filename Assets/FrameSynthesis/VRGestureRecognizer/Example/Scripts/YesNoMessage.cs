using UnityEngine;

namespace FrameSynthesis.VR.Example
{
    [RequireComponent(typeof(TextMesh))]
    public class YesNoMessage : MonoBehaviour
    {
        const string YesText = "Yes!";
        const string NoText = "No!";

        [SerializeField]
        float lifetime = 1f;
        [SerializeField]
        Vector3 velocity;

        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        public void Initialize(bool yesNo)
        {
            GetComponent<TextMesh>().text = yesNo ? YesText : NoText;
        }

        void Update()
        {
            transform.Translate(velocity * Time.deltaTime);
        }
    }
}
