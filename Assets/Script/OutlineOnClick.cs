using UnityEngine;

public class OutlineOnClick : MonoBehaviour
{
  private Renderer rend;
  private Material mat;

  public Color clickedOutline = Color.red;
  public float clickedWidth = 0.03f;

  private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
  private static readonly int OutlineWidth = Shader.PropertyToID("_Outline");

  public float forceMultiplier = 0.5f;
  public float maxVelocity = 20f;   // 최대 속도 제한 값

  private Rigidbody rb;
  private Camera cam;

  private Vector3 worldDown;
  private Vector3 worldUp;

  private float startY;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rend = GetComponent<Renderer>();
    mat = rend.material;
    cam = Camera.main;

    mat.SetColor(OutlineColor, Color.clear);
    mat.SetFloat(OutlineWidth, 0f);

    startY = transform.position.y;

    // 회전으로 넘어짐 방지 (X/Z 회전 고정)
    rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
  }

  private void OnMouseDown()
  {
    mat.SetColor(OutlineColor, clickedOutline);
    mat.SetFloat(OutlineWidth, clickedWidth);

    rb.isKinematic = true;

    // 드래그 시작 지점을 지면 평면(y=startY)으로 투영
    worldDown = MouseToGround(cam, Input.mousePosition, startY);
  }

  private void OnMouseUp()
  {
    mat.SetColor(OutlineColor, Color.clear);
    mat.SetFloat(OutlineWidth, 0f);

    rb.isKinematic = false;

    // 드래그 끝 지점을 지면으로 투영
    worldUp = MouseToGround(cam, Input.mousePosition, startY);

    // 방향 벡터 (드래그 반대 방향으로 발사)
    Vector3 delta = (worldDown - worldUp);
    delta.y = 0f; // 수평 성분만 사용 → 위로 힘 없음

    // 힘 적용
    rb.AddForce(delta * forceMultiplier, ForceMode.Impulse);

    // 속도 제한 적용
    if (rb.linearVelocity.magnitude > maxVelocity)
    {
      rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
    }
  }

  private static Vector3 MouseToGround(Camera cam, Vector3 mouse, float groundY)
  {
    Ray ray = cam.ScreenPointToRay(mouse);
    Plane ground = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
    if (ground.Raycast(ray, out float enter))
      return ray.GetPoint(enter);

    return cam.transform.position + cam.transform.forward * 10f;
  }
}