using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricGrid : MonoBehaviour
{
    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    [SerializeField] private float cellSize;

    private void OnDrawGizmos()
    {
        OnDrawGrid();
    }

    private void OnDrawGrid()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                Vector2 pos = StartPosition(x, y, cellSize);
                for (int i = 0; i < Corners().Length; i++)
                {
                    Gizmos.DrawLine(pos + Corners()[i % 4], pos + Corners()[(i + 1) % 4]);
                }
            }
        }
    }

    private Vector2 StartPosition(int x, int y, float size)
    {
        return new Vector2(x * GridWidth(size) / 2 + y * GridWidth(size) / 2,
            y * GridHeight(size) / 2 - x * GridHeight(size) / 2);
    }

    private float GridWidth(float size)
    {
        return size;
    }

    private float GridHeight(float size)
    {
        return size * 0.5f;
    }

    private Vector2[] Corners()
    {
        Vector2[] corners = new Vector2[4];

        corners[0] = new Vector2(0, 0);
        corners[1] = new Vector2(GridWidth(cellSize) / 2, -GridHeight(cellSize) / 2);
        corners[2] = new Vector2(GridWidth(cellSize), 0);
        corners[3] = new Vector2(GridWidth(cellSize) / 2, GridHeight(cellSize) / 2);

        return corners;
    }

    public static Vector2 WorldPosToIsoPos(Vector2 worldPos)
    {
        return new Vector2(worldPos.x - (int)worldPos.y / 2, worldPos.y / 2 + worldPos.x / 2);
    }
}