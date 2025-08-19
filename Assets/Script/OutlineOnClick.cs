using UnityEngine;

public class OutlineOnClick : MonoBehaviour
{
  private Renderer rend;
  private Rigidbody rb;
  private Camera cam;

  [Header("Outline")]
  public Color clickedOutline = Color.red;
  public float clickedWidth = 0.03f;

  [Header("Launch")]
  public float forceMultiplier = 0.5f; // 드래그 힘 배율
  public float maxVelocity = 20f;      // 발사 후 최대 속도

  private Vector3 dragStartWorld; // 드래그 시작점(지면 투영)
  private float groundY;          // 시작 시 높이(지면 기준)
  private bool pressed;

  void Awake()
  {
    rend = GetComponent<Renderer>();
    rb = GetComponent<Rigidbody>();
    cam = Camera.main;

    // 처음엔 아웃라인 끄기
    ClearOutline(rend);

    // 넘어짐 방지(필요 시)
    if (rb) rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
  }

  void Start()
  {
    groundY = transform.position.y;
  }

  void OnMouseDown()
  {
    if (!enabled || !gameObject.activeInHierarchy) return;

    // 클릭 시 아웃라인 켜기(아웃라인 속성 가진 머티리얼만)
    SetOutline(rend, clickedOutline, clickedWidth);

    if (rb) rb.isKinematic = true; // 드래그 동안 정지
    dragStartWorld = MouseToGround(cam, Input.mousePosition, groundY);
    pressed = true;
  }

  void OnMouseUp()
  {
    if (!pressed) return;
    pressed = false;

    // 아웃라인 끄기
    ClearOutline(rend);

    if (!rb) return;
    rb.isKinematic = false;

    // 드래그 끝 지점
    Vector3 dragEndWorld = MouseToGround(cam, Input.mousePosition, groundY);

    // 드래그 방향(끝→시작 = 반대 방향으로 발사), 수평 성분만
    Vector3 delta = dragStartWorld - dragEndWorld;
    delta.y = 0f;

    // 힘 적용
    rb.AddForce(delta * forceMultiplier, ForceMode.Impulse);

    // 속도 제한
    #if UNITY_6000_0_OR_NEWER
    if (rb.linearVelocity.magnitude > maxVelocity)
        rb.linearVelocity = rb.linearVelocity.normalized * maxVelocity;
    #else
    if (rb.velocity.magnitude > maxVelocity)
        rb.velocity = rb.velocity.normalized * maxVelocity;
    #endif
  }

  // ─────────────────────────────────────────────────────────────────────
  // Outline 유틸: 다중 머티리얼에서 _Outline / _OutlineColor 가진 슬롯만 제어
  void SetOutline(Renderer r, Color color, float width)
  {
    if (!r) return;
    // 클릭 강조는 일시적이므로 materials(인스턴스) 사용
    var mats = r.materials;
    for (int i = 0; i < mats.Length; i++)
    {
      var m = mats[i];
      if (!m) continue;

      if (m.HasProperty("_OutlineColor"))
        m.SetColor("_OutlineColor", color);
      if (m.HasProperty("_Outline"))
        m.SetFloat("_Outline", width);
    }
  }

  void ClearOutline(Renderer r)
  {
    if (!r) return;
    var mats = r.materials;
    for (int i = 0; i < mats.Length; i++)
    {
      var m = mats[i];
      if (!m) continue;

      if (m.HasProperty("_OutlineColor"))
        m.SetColor("_OutlineColor", Color.clear);
      if (m.HasProperty("_Outline"))
        m.SetFloat("_Outline", 0f);
    }
  }

  // ─────────────────────────────────────────────────────────────────────
  // 마우스 좌표를 y=groundY 평면으로 투영
  static Vector3 MouseToGround(Camera c, Vector3 mousePos, float groundY)
  {
    if (!c) return Vector3.zero;
    Ray ray = c.ScreenPointToRay(mousePos);
    Plane ground = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));
    if (ground.Raycast(ray, out float enter))
      return ray.GetPoint(enter);

    // 실패 시 카메라 앞 임의 지점
    return c.transform.position + c.transform.forward * 10f;
  }
}