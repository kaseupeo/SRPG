using System;
using UnityEngine;
using UnityEngine.Serialization;

public class HexGrid : MonoBehaviour
{
    [SerializeField] public HexOrientation orientation;
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] public float hexSize;
    [SerializeField] public GameObject hexPrefab;

    private void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 centrePosition = HexMetrics.Center(hexSize, x, y, orientation) + transform.position;

                for (int s = 0; s < HexMetrics.Corners(hexSize, orientation).Length; s++)
                {
                    Gizmos.DrawLine(
                        centrePosition + HexMetrics.Corners(hexSize, orientation)[s % 6],
                        centrePosition + HexMetrics.Corners(hexSize, orientation)[(s + 1) % 6]);
                }
            }
        }
    }
}

public enum HexOrientation
{
    FlatTop,
    PointyTop,
}