using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using ArrowDirection = Define.ArrowDirection;

public class MouseController : MonoBehaviour
{
    private GameObject _cursor;
    private List<Tile> _path;
    private List<Tile> _rangeFindingTiles;
    private bool _isMoving;
    
    // TODO : CharacterController 및 CharacterPrefab 완성 후 GameManager로 옮기기
    private Creature _creature;
    private List<PlayerCharacter> _playerCharacterPrefabs;
    
    
    private void Start()
    {
        GenerateCursor();
        _path = new List<Tile>();
        _rangeFindingTiles = new List<Tile>();
        _isMoving = false;
        
        _playerCharacterPrefabs = Managers.Game.PlayerCharacters;
        
    }

    private void LateUpdate()
    {
        MouseClick();

        if (_path.Count > 0 && _isMoving)
            MoveAlongPath();
    }

    private void GenerateCursor()
    {
        GameObject go = GameObject.Find("cursor");
        if (go == null)
        {
            GameObject cursorPrefab = Resources.Load("Prefabs/UI/Cursor") as GameObject;

            go = Instantiate(cursorPrefab, transform);
        }

        _cursor = go;
    }

    private void MouseClick()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            Tile tile = hit.Value.collider.GetComponent<Tile>();
            
            _cursor.transform.position = tile.transform.position;
            _cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;

            if (_rangeFindingTiles.Contains(tile) && !_isMoving)
            {
                _path.Clear();
                _path = PathFinding.FindPath(_creature.CurrentTile, tile, _rangeFindingTiles);
            
                foreach (Tile findingTile in _rangeFindingTiles)
                {
                    Managers.Map.MapTiles[findingTile.Grid2DLocation].SetSprite(ArrowDirection.None);
                }
            
                for (int i = 0; i < _path.Count; i++)
                {
                    var previousTile = i > 0 ? _path[i - 1] : _creature.CurrentTile;
                    var nextTile = i < _path.Count - 1 ? _path[i + 1] : null;
                    var arrow = TranslateDirection(previousTile, _path[i], nextTile);
                    _path[i].SetSprite(arrow);
                }
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                tile.ShowTile();

                // TODO : GameManager에서 GenerateCharacter()로 만들어서 옮기고 수정하기
                if (_creature == null)
                {
                    _creature = Instantiate(_playerCharacterPrefabs[0]).GetComponent<Creature>();
                    PositionCharacterOnLine(tile);
                    GetInRangeTiles();
                }
                else
                {
                    _isMoving = true;

                    foreach (Tile findingTile in _rangeFindingTiles)
                    {
                        findingTile.HideTile();
                    }
                    // tile.HideTile();
                }
            }
        }
    }
    
    private void MoveAlongPath()
    {
        float step = 5 * Time.deltaTime;
        float zIndex = _path[0].transform.position.z;

        _creature.transform.position =
            Vector2.MoveTowards(_creature.transform.position, _path[0].transform.position, step);
        _creature.transform.position =
            new Vector3(_creature.transform.position.x, _creature.transform.position.y, zIndex);

        if (Vector2.Distance(_creature.transform.position, _path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(_path[0]);
            _path.RemoveAt(0);
        }

        if (_path.Count == 0)
        {
            GetInRangeTiles();
            _isMoving = false;
        }
    }
    
    private RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();

        return null;
    }
    
    private void PositionCharacterOnLine(Tile tile)
    {
        _creature.transform.position =
            new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        _creature.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        _creature.CurrentTile = tile;
    }

    private void GetInRangeTiles()
    {
        // 이동 범위
        _rangeFindingTiles = RangeFinding.GetTilesInRange(_creature.CurrentTile.Grid2DLocation, 3);

        foreach (Tile tile in _rangeFindingTiles)
        {
            tile.ShowTile();
        }
    }

    // TODO : Tile.cs로 수정해서 옮기기
    public ArrowDirection TranslateDirection(Tile previousTile, Tile currentTile, Tile futureTile)
    {
        bool isFinal = futureTile == null;

        Vector2Int pastDirection = previousTile != null
            ? (Vector2Int)(currentTile.GridLocation - previousTile.GridLocation)
            : new Vector2Int(0, 0);
        Vector2Int futureDirection = futureTile != null
            ? (Vector2Int)(futureTile.GridLocation - currentTile.GridLocation)
            : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if (direction == new Vector2(0, 1) && !isFinal)
        {
            return ArrowDirection.Up;
        }

        if (direction == new Vector2(0, -1) && !isFinal)
        {
            return ArrowDirection.Down;
        }

        if (direction == new Vector2(1, 0) && !isFinal)
        {
            return ArrowDirection.Right;
        }

        if (direction == new Vector2(-1, 0) && !isFinal)
        {
            return ArrowDirection.Left;
        }

        if (direction == new Vector2(1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomLeft;
            else
                return ArrowDirection.TopRight;
        }

        if (direction == new Vector2(-1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomRight;
            else
                return ArrowDirection.TopLeft;
        }

        if (direction == new Vector2(1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopLeft;
            else
                return ArrowDirection.BottomRight;
        }

        if (direction == new Vector2(-1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopRight;
            else
                return ArrowDirection.BottomLeft;
        }

        if (direction == new Vector2(0, 1) && isFinal)
        {
            return ArrowDirection.UpFinished;
        }

        if (direction == new Vector2(0, -1) && isFinal)
        {
            return ArrowDirection.DownFinished;
        }

        if (direction == new Vector2(-1, 0) && isFinal)
        {
            return ArrowDirection.LeftFinished;
        }

        if (direction == new Vector2(1, 0) && isFinal)
        {
            return ArrowDirection.RightFinished;
        }

        return ArrowDirection.None;
    }

}
