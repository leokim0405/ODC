using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WallPlacer))]
public class WallPlacerEditor : Editor
{
    private bool[,] wallGrid;

    void OnEnable()
    {
        WallPlacer placer = (WallPlacer)target;
        int size = placer.gridSize;
        wallGrid = new bool[size, size];
    }

    public override void OnInspectorGUI()
    {
        WallPlacer placer = (WallPlacer)target;

        // 기본 필드 출력
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("벽 배치", EditorStyles.boldLabel);

        int size = placer.gridSize;

        // 그리드 토글 UI
        for (int y = size - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < size; x++)
            {
                wallGrid[x, y] = GUILayout.Toggle(wallGrid[x, y], "", GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        // 저장 버튼
        if (GUILayout.Button("벽 좌표 저장"))
        {
            List<Vector2Int> wallPositionsList = new List<Vector2Int>();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (wallGrid[x, y])
                        wallPositionsList.Add(new Vector2Int(x, y));
                }
            }

            placer.wallPositions = wallPositionsList.ToArray();
            EditorUtility.SetDirty(placer);
        }
    }
}