using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(WallPlacer))]
public class WallPlacerEditor : Editor
{
    private int[,] typeGrid;
    private int activeType = 0;
    private readonly string[] typeNames = { "Normal", "Bouncy", "Hazard" };
    private int lastSize = -1;

    void OnEnable()
    {
        var placer = (WallPlacer)target;
        SyncGridWithSizeAndSaved(placer);
    }

    public override void OnInspectorGUI()
    {
        var placer = (WallPlacer)target;
        if (lastSize != placer.gridSize)
            SyncGridWithSizeAndSaved(placer);

        DrawDefaultInspector();
        EditorGUILayout.Space();

        // 타입 선택 툴바
        EditorGUILayout.LabelField("벽 타입 선택", EditorStyles.boldLabel);
        activeType = GUILayout.Toolbar(activeType, typeNames);

        int size = Mathf.Max(1, placer.gridSize);
        float cell = 24f;

        EditorGUILayout.LabelField("벽 배치 그리드", EditorStyles.boldLabel);
        for (int y = size - 1; y >= 0; y--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < size; x++)
            {
                string label = CellLabel(typeGrid[x, y]);
                if (GUILayout.Button(label, GUILayout.Width(cell), GUILayout.Height(cell)))
                {
                    bool erase = Event.current.shift || Event.current.control;
                    typeGrid[x, y] = erase ? -1 : activeType;
                    GUI.FocusControl(null);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("벽 좌표 저장"))
        {
            Undo.RecordObject(placer, "Save Walls");
            var list = new List<WallEntry>();
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    if (typeGrid[x, y] >= 0)
                        list.Add(new WallEntry { coord = new Vector2Int(x, y), type = (WallType)typeGrid[x, y] });
            placer.walls = list.ToArray();
            EditorUtility.SetDirty(placer);
        }

        if (!Application.isPlaying && GUILayout.Button("씬에 즉시 배치(Place Walls)"))
        {
            placer.PlaceWalls();
        }
    }

    private void SyncGridWithSizeAndSaved(WallPlacer placer)
    {
        int size = Mathf.Max(1, placer.gridSize);
        EnsureGridSize(size);
        for (int x = 0; x < size; x++)
            for (int y = 0; y < size; y++)
                typeGrid[x, y] = -1;

        if (placer.walls != null)
            foreach (var w in placer.walls)
                if (InRange(w.coord, size))
                    typeGrid[w.coord.x, w.coord.y] = (int)w.type;

        lastSize = size;
    }

    private void EnsureGridSize(int size)
    {
        if (typeGrid != null && typeGrid.GetLength(0) == size && typeGrid.GetLength(1) == size) return;
        typeGrid = new int[size, size];
    }

    private bool InRange(Vector2Int c, int size) => c.x >= 0 && c.x < size && c.y >= 0 && c.y < size;
    private string CellLabel(int t) => t < 0 ? "" : typeNames[t][0].ToString();
}