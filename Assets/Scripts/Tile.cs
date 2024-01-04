using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int G;
    public int H;
    public int F => G + H;
    public bool IsBlocked;
    public Tile Previous;
    public Vector3Int GridLocation;
    
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