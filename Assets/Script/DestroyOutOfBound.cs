using UnityEngine;

public class DestroyOutOfBound : MonoBehaviour
{
    public float OutOfBound = -10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= OutOfBound)
        {
            Destroy(gameObject);
        }
    }
}
