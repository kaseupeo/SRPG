using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    public Node Node;
    
    private void Update()
    {
        // TODO : 수정필요
        if (Input.GetMouseButtonDown(0))
            HideTile();
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}

public class Node
{
    public int G;
    public int H;
    public int F => G + H;
    public bool IsBlocked;
    public OverlayTile Previous;
    public Vector3Int GridLocation;
}