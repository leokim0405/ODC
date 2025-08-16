using UnityEngine;

public class CameraView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 position = new Vector3(0, 150, 0);
        transform.position = position;

        Vector3 lookTarget = new Vector3(position.x, 0, position.z);
        transform.LookAt(lookTarget);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
