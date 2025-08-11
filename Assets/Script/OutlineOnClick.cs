using UnityEngine;

public class OutlineOnClick : MonoBehaviour
{
  private Renderer rend;

  private Material mat;
  public Color clickedOutline = Color.red;
  public float clickedWidth = 0.03f;

  private Color defaultColor = Color.clear;
  private float defaultWidth = 0f;

  private Color originalColor;

  public float forceMultiplier = 0.5f;
  private Rigidbody rb;
  private Vector3 mousePressDownPos;
  private Vector3 mouseReleasePos;

  private Camera cam;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    rend = GetComponent<Renderer>();
    mat = rend.material;

    originalColor = mat.color;

    mat.SetColor("_OutlineColor", defaultColor);
    mat.SetFloat("_Outline", defaultWidth);

    cam = Camera.main;
  }

  private void OnMouseDown()
  {
    mat.SetColor("_OutlineColor", clickedOutline);
    mat.SetFloat("_Outline", clickedWidth);

    mousePressDownPos = Input.mousePosition;
    rb.isKinematic = true;
  }

  void OnMouseUp()
  {
    mat.color = originalColor;

    mat.SetColor("_OutlineColor", defaultColor);
    mat.SetFloat("_Outline", defaultWidth);

    rb.isKinematic = false;
    mouseReleasePos = Input.mousePosition;

    Vector3 screenForce = mousePressDownPos - mouseReleasePos;
    Vector3 force = new Vector3(screenForce.x, 0, screenForce.y);

    Vector3 finalForce = force * forceMultiplier;
    // finalForce.y = 0;

    rb.AddForce(finalForce, ForceMode.Impulse);
  }

}
