using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(PrefabPlacer))]
public class PrefabPlacerEditor : Editor
{
    private bool[,] myGrid;
    private bool[,] opponentGrid;

    void OnEnable()
    {
        PrefabPlacer placer = (PrefabPlacer)target;
        int size = placer.gridSize;
        myGrid = new bool[size, size];
        opponentGrid = new bool[size, size];
    }

    public override void OnInspectorGUI()
    {
        PrefabPlacer placer = (PrefabPlacer)target;

        // 기본 Inspector
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("초기 배치 선택", EditorStyles.boldLabel);

        int size = placer.gridSize;

        // 내 알 배치
        EditorGUILayout.LabelField("내 알 (검정)");
        for (int y = size - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < size; x++)
            {
                myGrid[x, y] = GUILayout.Toggle(myGrid[x, y], "", GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        // 상대 알 배치
        EditorGUILayout.LabelField("상대 알 (흰색)");
        for (int y = size - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < size; x++)
            {
                opponentGrid[x, y] = GUILayout.Toggle(opponentGrid[x, y], "", GUILayout.Width(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        // 버튼 - 배열 업데이트
        if (GUILayout.Button("초기 좌표 저장"))
        {
            List<Vector2Int> myPositionsList = new List<Vector2Int>();
            List<Vector2Int> opponentPositionsList = new List<Vector2Int>();

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (myGrid[x, y])
                        myPositionsList.Add(new Vector2Int(x, y));
                    if (opponentGrid[x, y])
                        opponentPositionsList.Add(new Vector2Int(x, y));
                }
            }

            placer.myPositions = myPositionsList.ToArray();
            placer.opponentPositions = opponentPositionsList.ToArray();
            EditorUtility.SetDirty(placer);
        }
    }
}