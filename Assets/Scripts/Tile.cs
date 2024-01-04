using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;
    public bool IsBlocked { get; set; }
    public Tile Previous { get; set; }
    public Vector3Int GridLocation { get; set; }
    public Vector2Int Grid2DLocation => new Vector2Int(GridLocation.x, GridLocation.y);
    public List<Sprite> arrows;
    
    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
    }
    
    public void SetSprite(Define.ArrowDirection dir)
    {
        if (dir == Define.ArrowDirection.None)
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 0);
        else
        {
            GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(1, 1, 1, 1);
            GetComponentsInChildren<SpriteRenderer>()[1].sprite = arrows[(int)dir];
            GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        }
    }}