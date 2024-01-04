using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour
{
    private GameObject _cursor;

    // TODO : CharacterController 및 CharacterPrefab 완성 후 GameManager로 옮기기
    public GameObject characterPrefab;
    private Creature _creature;
    
    private List<Tile> _path;

    private List<PlayerCharacter> _playerCharacterPrefabs;

    private void Start()
    {
        GenerateCursor();
        _path = new List<Tile>();
        _playerCharacterPrefabs = Managers.Game.PlayerCharacters;
        
    }

    private void LateUpdate()
    {
        MouseClick();

        if (_path.Count > 0)
            MoveAlongPath();
    }

    private void GenerateCursor()
    {
        GameObject go = GameObject.Find("cursor");
        if (go == null)
        {
            GameObject cursorPrefab = Resources.Load("Prefabs/Cursor") as GameObject;

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
            
            if (Input.GetMouseButtonDown(0))
            {
                tile.ShowTile();

                // TODO : GameManager에서 GenerateCharacter()로 만들어서 옮기고 수정하기
                if (_creature == null)
                {
                    _creature = Instantiate(_playerCharacterPrefabs[0]).GetComponent<Creature>();
                    PositionCharacterOnLine(tile);
                    _creature.CurrentTile = tile;
                }
                else
                {
                    _path = PathFinding.FindPath(_creature.CurrentTile, tile);
                    tile.HideTile();
                }
            }
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
    }
}
