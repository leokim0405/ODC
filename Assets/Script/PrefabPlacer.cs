using UnityEngine;

public class PrefabPlacer : MonoBehaviour
{
    public GameObject prefab;            // 배치할 프리팹
    public Transform gridParent;          // GridDrawer의 부모 Transform
    public float cellSize = 1f;           // 셀 크기
    public float prefabHeight = 0.5f;     // 프리팹 높이 오프셋

    [Header("초기 좌표")]
    public Vector2Int[] myPositions;      // 내 알 위치
    public Vector2Int[] opponentPositions;// 상대 알 위치

    [Header("색상")]
    public Color myColor = Color.black;        // 내 알 색
    public Color opponentColor = Color.white;  // 상대 알 색

    [Header("보드 크기")]
    public int gridSize = 10;             // 격자 크기

    void Start()
    {
        PlaceMyPrefabs();
        PlaceOpponentPrefabs();
    }

    void PlaceMyPrefabs()
    {
        foreach (Vector2Int pos in myPositions)
        {
            Vector3 spawnPos = new Vector3(
                (pos.x * cellSize) + (cellSize / 2f),
                prefabHeight,
                (pos.y * cellSize) + (cellSize / 2f)
            );
            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity, gridParent);
            SetPrefabColor(obj, myColor);
        }
    }

    void PlaceOpponentPrefabs()
    {
        foreach (Vector2Int pos in opponentPositions)
        {
            Vector3 spawnPos = new Vector3(
                (pos.x * cellSize) + (cellSize / 2f),
                prefabHeight,
                (pos.y * cellSize) + (cellSize / 2f)
            );
            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity, gridParent);
            SetPrefabColor(obj, opponentColor);
        }
    }

    void SetPrefabColor(GameObject obj, Color color)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = color;
        }
    }
}