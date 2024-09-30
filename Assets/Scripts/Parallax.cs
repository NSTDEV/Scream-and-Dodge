using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float scrollSpeed = -5f;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
        startPos.y = 14f;
    }

    void Update()
    {
        float newPos = Mathf.Repeat(Time.time * scrollSpeed, startPos.y);
        transform.position = new Vector3(startPos.x, startPos.y - newPos, startPos.z);
    }
}
