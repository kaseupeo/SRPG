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
    private PlayerActions _playerActions;
    
    [CanBeNull] private Tile _focusTile;
    private Cursor _cursor;
    private List<Tile> _path;
    private List<Tile> _rangeFindingTiles;
    private bool _isMoving;
    
    // TODO : CharacterController 및 CharacterPrefab 완성 후 GameManager로 옮기기
    private PlayerCharacter _selectedPlayerCharacter;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        GenerateCursor();
    }

    private void Start()
    {
        _playerActions.Mouse.Click.performed += _ => MouseClick();
        _path = new List<Tile>();
        _rangeFindingTiles = new List<Tile>();
        _isMoving = false;
    }

    private void LateUpdate()
    {
        // 정리 중인 코드
        _focusTile = GetMousePositionOnTile();
        SetCursor(_focusTile);
        
        
        // GetInRangeTiles(_selectedPlayerCharacter);
        ShowFindPath(_focusTile, _selectedPlayerCharacter);

            // MoveAlongPath(_selectedPlayerCharacter);
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
    private Tile GetMousePositionOnTile()
    {
        Vector2 mousePosition = _playerActions.Mouse.Position.ReadValue<Vector2>();
        RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);

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
    
    // 마우스
    private void MouseClick()
    {
        switch (Managers.Game.GameMode)
        {
            case Define.GameMode.Preparation:
                Managers.Game.GeneratePlayerCharacter(_focusTile);
                break;
            case Define.GameMode.Battle:

                SelectedTileInfo(_focusTile);
                GetInRangeTiles(_selectedPlayerCharacter);


                if (_selectedPlayerCharacter != null && _focusTile != _selectedPlayerCharacter.CurrentTile)
                {
                    foreach (Tile findingTile in _rangeFindingTiles)
                    {
                        if (_focusTile == findingTile)
                        {
                            _isMoving = true;

                            StartCoroutine(MoveAlongPath(_selectedPlayerCharacter));
                            break;
                        }
                    }
                    HideRangeTiles();
                    _selectedPlayerCharacter = null;
                }
                
                break;
        }
    }
    
    private void SelectedTileInfo(Tile tile)
    {
        if (tile == null || !tile.IsBlocked)
            return;

        foreach (var playerCharacter in Managers.Game.PlayerCharacters.Where(playerCharacter => playerCharacter.CurrentTile == tile))
        {
            _selectedPlayerCharacter = playerCharacter;
        }
    }
    
    // 캐릭터 이동 메소드
    private IEnumerator MoveAlongPath(PlayerCharacter playerCharacter)
    {
        while (true)
        {
            yield return null;

            playerCharacter.Move(_path[0]);

            if (Vector2.Distance(playerCharacter.transform.position, _path[0].transform.position) < 0.00001f)
            {
                playerCharacter.CharacterPositionOnTile(_path[0]);
                _path.RemoveAt(0);
            }

            if (_path.Count == 0)
            {
                _selectedPlayerCharacter = null;
                _isMoving = false;
            }

            if (_path.Count <= 0 || !_isMoving)
            {
                _path.Clear();
                yield break;
            }
        }
    }

    // 캐릭터의 이동 범위 찾고 보여주는 메소드
    private void GetInRangeTiles(PlayerCharacter playerCharacter)
    {
        HideRangeTiles();
        
        if (playerCharacter == null)
            return;
        
        // TODO : 캐릭터 
        // 이동 범위
        _rangeFindingTiles = RangeFinding.GetTilesInRange(playerCharacter.CurrentTile.Grid2DLocation, 4);

        foreach (Tile tile in _rangeFindingTiles) 
            tile.ShowTile();
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

    private void HideRangeTiles()
    {
        foreach (Tile tile in _rangeFindingTiles) 
            tile.HideTile();
    }
    
    private void OnEnable()
    {
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
    }
}

   
// 전투 준비 중
// 타일 클릭 시 -> 캐릭터 배치
// 배치된 캐릭터 선택 후 다른 타일 클릭시 재배치
    
// 전투 중
// 맵에 있는 캐릭터 선택 -> 기본으로 이동범위 표시
// 캐릭터 선택 상태에서
// -> 다른 캐릭터 선택시 해당 캐릭터 선택
// -> 캐릭터 이동 범위 내 선택시 이동
// -> 캐릭터 이동 범위 외 선택시 캐릭터 선택 취소
// private void MouseClick(Tile clickTile)
// {
//     if (Input.GetMouseButtonDown(0))
//     {
//         if (clickTile == null)
//             return;
//         
//         
//         if (!_isSelectedPlayerCharacter)
//         {
//             // _selectedPlayerCharacter = Managers.Game.SelectedPlayerCharacter(clickTile);
//             _isSelectedPlayerCharacter = true;
//             GetInRangeTiles(_selectedPlayerCharacter);
//         }
//         
//         
//         // TODO : 널 오류 뜸 수정 필요
//         if (clickTile != _selectedPlayerCharacter.CurrentTile && _isSelectedPlayerCharacter)
//         {
//             _isSelectedPlayerCharacter = false;
//
//             foreach (Tile findingTile in _rangeFindingTiles)
//             {
//                 if (clickTile == findingTile)
//                 {
//                     _isMoving = true;
//                     _isSelectedPlayerCharacter = true;
//                     
//                     // MoveAlongPath(_playerCharacter);
//                 }
//                 findingTile.HideTile();
//         
//             }
//         }
//         
//         
//     }
// }