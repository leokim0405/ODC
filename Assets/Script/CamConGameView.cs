using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 5f;

    void Update()
    {
        // 이동 (WASD)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(h, 0, v) * moveSpeed * Time.deltaTime);

        // 마우스 회전 (우클릭 시)
        if (Input.GetMouseButton(1)) // 오른쪽 마우스 누른 상태
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            transform.Rotate(new Vector3(mouseY, mouseX, 0) * rotateSpeed);
        }
    }
}