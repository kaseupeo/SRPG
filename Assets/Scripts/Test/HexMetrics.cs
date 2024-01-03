using UnityEngine;

public class HexMetrics
{
    public static float OuterRadius(float hexSize)
    {
        return hexSize;
    }

    public static float InnerRadius(float hexSize)
    {
        return hexSize * Mathf.Sqrt(3) / 2;
    }

    public static Vector3[] Corners(float hexSize, HexOrientation orientation)
    {
        Vector3[] corners = new Vector3[6];

        for (int i = 0; i < 6; i++)
        {
            corners[i] = Corner(hexSize, orientation, i);
        }

        return corners;
    }

    public static Vector3 Corner(float hexSize, HexOrientation orientation, int index)
    {
        float angle = 60f * index;

        if (orientation == HexOrientation.PointyTop)
        {
            angle += 30f;
        }

        Vector3 corner = new Vector3(hexSize * Mathf.Cos(angle * Mathf.Deg2Rad),
            hexSize * Mathf.Sin(angle * Mathf.Deg2Rad), 0f
        );

        return corner;
    }

    public static Vector3 Center(float hexSize, int x, int y, HexOrientation orientation)
    {
        Vector3 centrePosition;

        if (orientation == HexOrientation.PointyTop)
        {
            centrePosition.x = (x + y * 0.5f - y / 2) * (InnerRadius(hexSize) * 2f);
            centrePosition.y = y * (OuterRadius(hexSize) * 1.5f);
            centrePosition.z = 0f;
        }
        else
        {
            centrePosition.x = x * (OuterRadius(hexSize) * 1.5f);
            centrePosition.y = (y + x * 0.5f - x / 2) * (InnerRadius(hexSize) * 2f);
            centrePosition.z = 0f;
        }

        return centrePosition;
    }
}