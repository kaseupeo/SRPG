using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Isometric))]
public class GridManager : Editor
{
    private SerializedProperty _mapSize;
    private SerializedProperty _cellSize;
    
    private void OnEnable()
    {
        _mapSize = serializedObject.FindProperty("mapSize");
        _cellSize = serializedObject.FindProperty("cellSize");
    }
    
    private void OnSceneGUI()
    {
        
    }

    private float GridWidth(float size)
    {
        return size;
    }

    private float GridHeight(float size)
    {
        return size * 0.5f;
    }
    
    // private void DrawGrid()
    // {
    //     Vector2 startPos = Isometric.position;
    //     Vector2 endPos = transform.position;
    //     
    //     startPos.y += GridHeight(_cellSize);
    //     endPos.x -= height * GridWidth(_cellSize);
    //     endPos.y -= (height - 1) * GridHeight(_cellSize);
    //     
    //     for (int x = 0; x < width; x++)
    //     {
    //         startPos.x += GridWidth(_cellSize);
    //         startPos.y -= GridHeight(_cellSize);
    //         endPos.x += GridWidth(_cellSize);
    //         endPos.y -= GridHeight(_cellSize);
    //     }
    //     
    //     startPos = transform.position;
    //     endPos = transform.position;
    //     
    //     startPos.y += GridHeight(_cellSize);
    //     endPos.x += width * GridWidth(_cellSize);
    //     endPos.y -= (width - 1) * GridHeight(_cellSize);
    //     
    //     for (int y = 0; y < width; y++)
    //     {
    //         startPos.x -= GridWidth(_cellSize);
    //         startPos.y -= GridHeight(_cellSize);
    //         endPos.x -= GridWidth(_cellSize);
    //         endPos.y -= GridHeight(_cellSize);
    //     }
    //     
    // }
    

    // public static float IsoGridWidth(float size)
    // {
    //     return size;
    // }
    //
    // public static float IsoGridHeight(float size)
    // {
    //     return size / 2;
    // }
    //
    // public static Vector3 Corner(int index, int size = 1)
    // {
    //     float angle;
    //     if (index % 2 == 0)
    //         angle = 60f * index;
    //     else
    //         angle = 60f * 2 * index;
    //     
    //     Vector3 corner = new Vector3(size * Mathf.Cos(angle * Mathf.Deg2Rad), size * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    //
    //     return corner;
    // }
    //
    // public static Vector3[] Corners(int size = 1)
    // {
    //     Vector3[] corners = new Vector3[4];
    //
    //     for (int i = 0; i < 4; i++)
    //     {
    //         corners[i] = Corner(i, size);
    //     }
    //
    //     return corners;
    // }
    
    
}