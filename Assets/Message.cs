using UnityEngine;

public class Message : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f);
    }

    public void SetText(string text)
    {
        GetComponent<TextMesh>().text = text;
    }

    void Update()
    {
        Vector3 p = transform.localPosition;
        p.y += 1f * Time.deltaTime;
        transform.localPosition = p;
    }
}


