using UnityEngine;

public class GridDrawer : MonoBehaviour
{
    public int gridSize = 10;
    public float cellSize = 1f;
    public GameObject floorPrefab;
    public Vector3 gridOffset = Vector3.zero; // 🔹 격자 전체 위치 조정

    void Start()
    {
        PlaceFloorTiles();
    }

    void PlaceFloorTiles()
    {
        if (floorPrefab == null)
        {
            Debug.LogError("Floor FBX Prefab이 할당되지 않았습니다!");
            return;
        }

        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                // 각 타일 위치 계산 + 오프셋 적용
                Vector3 tilePosition = new Vector3(
                    x * cellSize,
                    0f,
                    z * cellSize
                ) + gridOffset;

                GameObject tile = Instantiate(floorPrefab, tilePosition, Quaternion.identity, this.transform);
                tile.transform.localScale = Vector3.one * cellSize;
            }
        }
    }
}
