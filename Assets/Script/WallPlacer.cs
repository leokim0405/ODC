using UnityEngine;

public enum WallType
{
    None = -1,
    Normal = 0,
    Bouncy = 1,
    Hazard = 2
}

[System.Serializable]
public struct WallEntry
{
    public Vector2Int coord;
    public WallType type;
}

public class WallPlacer : MonoBehaviour
{
    public int gridSize = 9;
    public float cellSize = 1f;
    public WallEntry[] walls;
    public GameObject[] wallPrefabs;

    [ContextMenu("Place Walls")]
    public void PlaceWalls()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
            DestroyImmediate(transform.GetChild(i).gameObject);

        if (walls == null || wallPrefabs == null) return;

        foreach (var w in walls)
        {
            if (w.type == WallType.None) continue;
            int idx = (int)w.type;
            if (idx < 0 || idx >= wallPrefabs.Length) continue;
            if (wallPrefabs[idx] == null) continue;

            Vector3 pos = new Vector3(w.coord.x * cellSize, 0f, w.coord.y * cellSize);
            Instantiate(wallPrefabs[idx], pos, Quaternion.identity, transform);
        }
    }
}