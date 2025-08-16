using UnityEngine;

public class WallPlacer : MonoBehaviour
{
    public int gridSize = 9;
    public Vector2Int[] wallPositions;

    public GameObject wallPrefab;

    public float cellSize = 1.0f;

    void Start()
    {
        PlaceWalls();
    }

    void PlaceWalls()
    {
        foreach (Vector2Int pos in wallPositions)
        {
            Vector3 worldPos = new Vector3(pos.x * cellSize, 0f, pos.y * cellSize);
            Instantiate(wallPrefab, worldPos, Quaternion.identity, this.transform);
        }
    }
}
