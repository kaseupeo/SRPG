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
    private CharacterController _character;
    
    private List<OverlayTile> _path;

    private void Start()
    {
        GenerateCursor();
        _path = new List<OverlayTile>();
    }

    private void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            OverlayTile tile = hit.Value.collider.GetComponent<OverlayTile>();
            
            _cursor.transform.position = tile.transform.position;
            _cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
            
            if (Input.GetMouseButtonDown(0))
            {
                tile.ShowTile();

                // TODO : GameManager에서 GenerateCharacter()로 만들어서 옮기고 수정하기
                if (_character == null)
                {
                    _character = Instantiate(characterPrefab).GetComponent<CharacterController>();
                    PositionCharacterOnLine(tile);
                    _character.standingOnTile = tile;
                }
                else
                {
                    _path = PathFinding.FindPath(_character.standingOnTile, tile);
                    tile.HideTile();
                }
            }
        }

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
    
    private RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();

        return null;
    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        _character.transform.position =
            new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        _character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        _character.standingOnTile = tile;
    }

    private void MoveAlongPath()
    {
        float step = 5 * Time.deltaTime;
        float zIndex = _path[0].transform.position.z;

        _character.transform.position =
            Vector2.MoveTowards(_character.transform.position, _path[0].transform.position, step);
        _character.transform.position =
            new Vector3(_character.transform.position.x, _character.transform.position.y, zIndex);

        if (Vector2.Distance(_character.transform.position, _path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(_path[0]);
            _path.RemoveAt(0);
        }
    }
}
