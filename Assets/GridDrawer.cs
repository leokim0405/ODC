using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    public int gridSize = 10;
    public float cellSize = 1f;
    public Material lineMaterial;

    void Start()
    {
        DrawGrid();
    }

    void DrawGrid()
    {
        for (int i = 0; i <= gridSize; i++)
        {
            // 세로선
            CreateLine(
                new Vector3(i * cellSize, 0, 0),
                new Vector3(i * cellSize, 0, gridSize * cellSize)
            );

            // 가로선
            CreateLine(
                new Vector3(0, 0, i * cellSize),
                new Vector3(gridSize * cellSize, 0, i * cellSize)
            );
        }
    }

    void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineObj = new GameObject("GridLine");
        lineObj.transform.parent = this.transform;

        LineRenderer lr = lineObj.AddComponent<LineRenderer>();
        lr.material = lineMaterial;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.useWorldSpace = true;
    }
}