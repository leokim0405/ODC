using UnityEngine;

public class OutlineOnClick : MonoBehaviour
{
  private Renderer rend;
  private Material mat;

  public Color clickedOutline = Color.red;
  public float clickedWidth = 0.03f;

  private Color defaultColor = Color.clear;
  private float defaultWidth = 0f;

  private Camera cam;

  void Start()
  {
    rend = GetComponent<Renderer>();
    mat = rend.material;

    mat.SetColor("_OutlineColor", defaultColor);
    mat.SetFloat("_Outline", defaultWidth);

    cam = Camera.main;
  }

  void Update()
  {
    if (Input.GetMouseButton(0))
    {
      Ray ray = cam.ScreenPointToRay(Input.mousePosition);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit))
      {
        if (hit.transform == transform)
        {
          mat.SetColor("_OutlineColor", clickedOutline);
          mat.SetFloat("_Outline", clickedWidth);
          return;
        }
      }
    }

    mat.SetColor("_OutlineColor", defaultColor);
    mat.SetFloat("Outline", defaultWidth);
    
  }

  // void OnMouseDown()
  // {

  //   Debug.Log("click\n");
  //   // Toggle outline color
  //   Color current = mat.GetColor("_OutlineColor");
  //   mat.SetColor("_OutlineColor", current == clickedOutline ? defaultOutline : clickedOutline);
  // }

  // void OnMouseUp()
  // {
  //   mat.SetColor("_Outlinecolor", defaultOutline);
  // }

}
