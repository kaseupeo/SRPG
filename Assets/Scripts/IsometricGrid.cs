using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricGrid : MonoBehaviour
{
    [SerializeField] private float height;
    [SerializeField] private float width;
    [SerializeField] private float size;

    private void OnDrawGizmos()
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = transform.position;

        startPos.y += GridHeight(size);
        endPos.x -= height * GridWidth(size);
        endPos.y -= (height - 1) * GridHeight(size);
        Gizmos.DrawLine(startPos, endPos);

        for (int x = 0; x < width; x++)
        {
            startPos.x += GridWidth(size);
            startPos.y -= GridHeight(size);
            endPos.x += GridWidth(size);
            endPos.y -= GridHeight(size);
            Gizmos.DrawLine(startPos, endPos);
        }

        startPos = transform.position;
        endPos = transform.position;

        startPos.y += GridHeight(size);
        endPos.x += width * GridWidth(size);
        endPos.y -= (width - 1) * GridHeight(size);
        Gizmos.DrawLine(startPos, endPos);

        for (int y = 0; y < width; y++)
        {
            startPos.x -= GridWidth(size);
            startPos.y -= GridHeight(size);
            endPos.x -= GridWidth(size);
            endPos.y -= GridHeight(size);
            Gizmos.DrawLine(startPos, endPos);
        }
        
        
    }


    private float GridWidth(float size)
    {
        return size;
    }

    private float GridHeight(float size)
    {
        return size * 0.5f;
    }

    private void OnDrawGrid()
    {
    }
}


// startPos.x += width;
// startPos.y -= height;
// endPos.x += width;
// endPos.y -= height;