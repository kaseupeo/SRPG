using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using ArrowDirection = Define.ArrowDirection;

public class MouseController : MonoBehaviour
{
    [CanBeNull] private Tile _focusTile;
    private Cursor _cursor;
    private List<Tile> _path;
    private List<Tile> _rangeFindingTiles;
    private bool _isMoving;
    private bool _isSelectedPlayerCharacter;
    
    // TODO : CharacterController 및 CharacterPrefab 완성 후 GameManager로 옮기기
    private PlayerCharacter _playerCharacter;
    private List<PlayerCharacter> _playerCharacterPrefabs;
    
    // TODO : 경로 변수 옮길지 고민하기, 플레이어 변수 삭제 필요
    
    private void Start()
    {
        GenerateCursor();
        _path = new List<Tile>();
        _rangeFindingTiles = new List<Tile>();
        _isMoving = false;
        
        _playerCharacterPrefabs = Managers.Game.PlayerCharacterPrefabs;
    }

    private void LateUpdate()
    {
        _focusTile = GetFocusedOnTile();

        SetCursor(_focusTile);
        MouseClick(_focusTile);

        if (_path.Count > 0 && _isMoving)
            MoveAlongPath(_playerCharacter);
    }

    // 커서 생성 메소드
    private void GenerateCursor()
    {
        GameObject go = GameObject.Find("cursor");
        if (go == null)
        {
            GameObject cursorPrefab = Resources.Load("Prefabs/UI/Cursor") as GameObject;

            go = Instantiate(cursorPrefab, transform);
        }

        _cursor = go.GetComponent<Cursor>();
    }
    
    // 마우스있는 곳에 위치한 타일 찾는 메소드
    [CanBeNull]
    private Tile GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length <= 0) 
            return null;
        
        RaycastHit2D? hit = hits.OrderByDescending(i => i.collider.transform.position.z).First();
        Tile tile = hit.Value.collider.GetComponent<Tile>();
            
        return tile;
    }

    // 마우스 위치에 커서 표시 메소드 
    private void SetCursor(Tile focusTile)
    {
        if (focusTile == null)
        {
            _cursor.HideCursor();
            return;
        }
        
        _cursor.transform.position = focusTile.transform.position;
        _cursor.GetComponent<SpriteRenderer>().sortingOrder = focusTile.GetComponent<SpriteRenderer>().sortingOrder;
        _cursor.ShowCursor();
    }

    private int testCheck = 0;
    
    // 마우스
    private void MouseClick(Tile clickTile)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (clickTile == null)
                return;
            
            
            // TODO : 전투(게임) 시작전에 캐릭터 배치 하는 메소드 따로 만들어서 관리하기
            if (testCheck < 2)
            {
                Managers.Game.GeneratePlayerCharacter(_playerCharacterPrefabs[testCheck], clickTile);
                testCheck++;
                return;
            }
            
            if (!_isSelectedPlayerCharacter)
            {
                _playerCharacter = Managers.Game.SelectedPlayerCharacter(clickTile);
                _isSelectedPlayerCharacter = true;
                GetInRangeTiles(_playerCharacter);
            }
            
            
            // TODO : 널 오류 뜸 수정 필요
            if (clickTile != _playerCharacter.CurrentTile && _isSelectedPlayerCharacter)
            {
                _isSelectedPlayerCharacter = false;

                foreach (Tile findingTile in _rangeFindingTiles)
                {
                    if (clickTile == findingTile)
                    {
                        _isMoving = true;
                        _isSelectedPlayerCharacter = true;
                        
                        // MoveAlongPath(_playerCharacter);
                    }
                    findingTile.HideTile();
            
                }
            }
            
            
        }
    }
    
    // 캐릭터 이동 메소드
    private void MoveAlongPath(PlayerCharacter playerCharacter)
    {
        playerCharacter.Move(_path[0]);
        
        if (Vector2.Distance(playerCharacter.transform.position, _path[0].transform.position) < 0.00001f)
        {
            // PositionCharacterOnLine(_path[0]);
            playerCharacter.CharacterPositionOnTile(_path[0]);
            _path.RemoveAt(0);
        }

        if (_path.Count == 0)
        {
            GetInRangeTiles(playerCharacter);
            _isMoving = false;
        }
    }

    // 캐릭터의 이동 범위 찾고 보여주는 메소드
    private void GetInRangeTiles(PlayerCharacter playerCharacter)
    {
        if (playerCharacter == null)
            return;
        
        // TODO : 캐릭터 
        // 이동 범위
        _rangeFindingTiles = RangeFinding.GetTilesInRange(playerCharacter.CurrentTile.Grid2DLocation, 4);

        foreach (Tile tile in _rangeFindingTiles) 
            tile.ShowTile();

        ShowFindPath(_focusTile, playerCharacter);
    }
    
    // 찾은 경로 보여주기
    private void ShowFindPath(Tile focusTile, PlayerCharacter playerCharacter)
    {
        if (_rangeFindingTiles.Contains(focusTile) && !_isMoving && playerCharacter != null)
        {
            _path.Clear();
            _path = PathFinding.FindPath(playerCharacter.CurrentTile, focusTile, _rangeFindingTiles);
            
            foreach (Tile findingTile in _rangeFindingTiles)
            {
                Managers.Map.MapTiles[findingTile.Grid2DLocation].SetSprite(ArrowDirection.None);
            }
            
            for (int i = 0; i < _path.Count; i++)
            {
                Tile prevTile = i > 0 ? _path[i - 1] : playerCharacter.CurrentTile;
                Tile nextTile = i < _path.Count - 1 ? _path[i + 1] : null;
                ArrowDirection arrow = _path[i].TranslateDirection(prevTile, nextTile);
                _path[i].SetSprite(arrow);
            }
        }
    }
    
    // private void Check
}
