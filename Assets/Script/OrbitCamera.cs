using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    public Transform target;        // 카메라가 바라볼 중심점 (보드 중앙)
    public float distance = 30f;    // 중심점과의 거리
    public float xSpeed = 120f;     // 좌우 회전 속도
    public float ySpeed = 80f;      // 상하 회전 속도

    public float yMinLimit = 10f;   // 상하 회전 최소 각도 (지면 근처)
    public float yMaxLimit = 80f;   // 상하 회전 최대 각도 (거의 수직에서 내려다보는 시점)

    public float distanceMin = 10f; // 최소 줌 거리
    public float distanceMax = 50f; // 최대 줌 거리

    private float x = 0.0f;
    private float y = 45.0f;        // 기본 각도: 약간 위에서 내려다보는 시점

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        if (target == null)
        {
            Debug.LogError("📌 OrbitCamera: target이 설정되지 않았습니다!");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (Input.GetMouseButton(1)) // 우클릭 회전
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);
        }

        // 마우스 휠로 줌 (옵션)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance - scroll * 10f, distanceMin, distanceMax);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}