using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ArrowDirection = Define.ArrowDirection;

public class Tile : MonoBehaviour
{
    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;
    // public bool IsBlocked { get; set; }
    public Tile Previous { get; set; }
    public Vector3Int GridLocation { get; set; }
    public Vector2Int Grid2DLocation => new Vector2Int(GridLocation.x, GridLocation.y);
    
    [SerializeField]
    private List<Sprite> arrows;

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _childSpriteRenderer;

    private bool _isBlocked;
    
    public bool IsBlocked { get => _isBlocked; set => _isBlocked = value; }

    private int _saveZ;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _childSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
    }

    private void Update()
    {
        // ChangeZPosition();
    }

    public void ShowTile()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void HideTile()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        _childSpriteRenderer.color = new Color(1, 1, 1, 0);
    }

    public void SetSprite(ArrowDirection dir)
    {
        if (dir == ArrowDirection.None)
            _childSpriteRenderer.color = new Color(1, 1, 1, 0);
        else
        {
            _childSpriteRenderer.color = new Color(1, 1, 1, 1);
            _childSpriteRenderer.sprite = arrows[(int)dir];
            _childSpriteRenderer.sortingOrder = _spriteRenderer.sortingOrder;
        }
    }

    private void ChangeZPosition()
    {
        var i = GridLocation;
        i.z = _isBlocked ? -100 : _saveZ;
        GridLocation = i;

        // Managers.Map.UpdateTileInfo(this);
        // var vector3 = transform.position;
        // vector3.z = _isOnTile ? -100 : _saveZ;
        // transform.position = vector3;
    }

    public ArrowDirection TranslateDirection(Tile prevTile, Tile nextTile)
    {
        bool isFinal = nextTile == null;

        Vector2Int pastDirection = prevTile != null
            ? (Vector2Int)(GridLocation - prevTile.GridLocation)
            : new Vector2Int(0, 0);
        Vector2Int nextDirection = nextTile != null
            ? (Vector2Int)(nextTile.GridLocation - GridLocation)
            : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != nextDirection ? pastDirection + nextDirection : nextDirection;
        
        if (direction == new Vector2(0,1))
            return isFinal ? ArrowDirection.UpFinished : ArrowDirection.Up;

        if (direction == new Vector2(0,-1))
            return isFinal ? ArrowDirection.DownFinished : ArrowDirection.Down;

        if (direction == new Vector2(1,0))
            return isFinal ? ArrowDirection.RightFinished : ArrowDirection.Right;

        if (direction == new Vector2(-1, 0))
            return isFinal ? ArrowDirection.LeftFinished : ArrowDirection.Left;

        if (direction == new Vector2(1, 1))
            return pastDirection.y < nextDirection.y ? ArrowDirection.BottomLeft : ArrowDirection.TopRight;

        if (direction == new Vector2(-1, 1))
            return pastDirection.y < nextDirection.y ? ArrowDirection.BottomRight : ArrowDirection.TopLeft;

        if (direction == new Vector2(1, -1))
            return pastDirection.y > nextDirection.y ? ArrowDirection.TopLeft : ArrowDirection.BottomRight;

        if (direction == new Vector2(-1, -1))
            return pastDirection.y > nextDirection.y ? ArrowDirection.TopRight : ArrowDirection.BottomLeft;

        return ArrowDirection.None;
    }
}