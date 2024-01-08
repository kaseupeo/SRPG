using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;
using ArrowDirection = Define.ArrowDirection;

public class PlayerController : MonoBehaviour
{
    private PlayerActions _playerActions;
    
    [CanBeNull] private Tile _focusTile;
    private Cursor _cursor;
    private List<Tile> _path;
    private List<Tile> _rangeFindingTiles;
    private bool _isMoving;
    private bool _isAttack;
    private PlayerCharacter _selectedPlayerCharacter;
    private Monster _selectedMonster;

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
        _isAttack = false;
    }

    private void LateUpdate()
    {
        _focusTile = GetMousePositionOnTile();
        SetCursor(_focusTile);

        switch (Managers.Game.State)
        {
            case Define.State.Move:
                GetInRangeTiles(_selectedPlayerCharacter);
                ShowFindPath(_focusTile, _selectedPlayerCharacter);
                break;
            case Define.State.Attack:
                GetInAttackRangeTiles(_selectedPlayerCharacter);
                break;
            case Define.State.Dead:
                break;
        }
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
    
    // 마우스
    private void MouseClick()
    {
        switch (Managers.Game.GameMode)
        {
            case Define.GameMode.Preparation:
                Managers.Game.GeneratePlayerCharacter(_focusTile);
                break;
            case Define.GameMode.PlayerTurn:
                
                SelectedTileInfo(_focusTile);

                switch (Managers.Game.State)
                {
                    case Define.State.Move:
                        Debug.Log($"{Managers.Game.State}");
                        MoveCheck();
                        break;
                    case Define.State.Attack:
                        Debug.Log($"{Managers.Game.State}");
                        MonsterCheck();
                        break;
                    case Define.State.Dead:
                        Debug.Log($"{Managers.Game.State}");
                        break;
                }
                
                break;
        }
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
    
    // 선택한 타일 정보(캐릭터) 
    private void SelectedTileInfo(Tile tile)
    {
        if (tile == null || !tile.IsBlocked || _isMoving)
            return;

        foreach (var playerCharacter in Managers.Game.PlayerCharacters.Where(playerCharacter => playerCharacter.CurrentTile == tile))
        {
            _selectedPlayerCharacter = playerCharacter;
        }

        foreach (var monster in Managers.Game.Monsters.Where(monster => monster.CurrentTile == tile))
        {
            _selectedMonster = monster;
        }
    }

    // 캐릭터의 이동 범위 또는 공격 범위 찾고 보여주는 메소드
    private void GetInRangeTiles(PlayerCharacter playerCharacter)
    {
        HideRangeTiles();
        
        if (playerCharacter == null)
            return;
        
        _rangeFindingTiles = PathFinding.GetTilesInRange(playerCharacter.CurrentTile.Grid2DLocation, playerCharacter.CurrentTurnCost);


        foreach (Tile tile in _rangeFindingTiles)
            tile.ShowTile();
        
        playerCharacter.CurrentTile.HideTile();
    }

    private void GetInAttackRangeTiles(PlayerCharacter playerCharacter)
    {
        HideRangeTiles();

        if (playerCharacter == null)
            return;

        // TODO : 캐릭터의 공격 범위 가져오기
        _rangeFindingTiles = PathFinding.GetTilesInRange(playerCharacter.CurrentTile.Grid2DLocation, 1);

        foreach (Tile tile in _rangeFindingTiles) 
            tile.ShowAttackTile();
        
        playerCharacter.CurrentTile.HideTile();
    }
    
    // 이동 체크 후 이동
    private void MoveCheck()
    {
        if (_selectedPlayerCharacter == null || _focusTile == _selectedPlayerCharacter.CurrentTile) return;
        if (_rangeFindingTiles.Any(findingTile => _focusTile == findingTile))
        {
            _isMoving = true;

            StartCoroutine(MoveAlongPath(_selectedPlayerCharacter));
        }
        
        HideRangeTiles();
        // _selectedPlayerCharacter = null;
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
                // _selectedPlayerCharacter = null;
                _isMoving = false;
            }

            if (_path.Count <= 0 || !_isMoving)
            {
                _path.Clear();
                yield break;
            }
        }
    }

    // 몬스터 체크
    private void MonsterCheck()
    {
        if (_selectedPlayerCharacter == null || _selectedMonster == null || _focusTile == _selectedPlayerCharacter.CurrentTile)
            return;
        foreach (var tile in _rangeFindingTiles.Where(tile => tile == _selectedMonster.CurrentTile))
        {
            _selectedPlayerCharacter.Attack(tile);
            _selectedMonster = null;
        }
    }
    
    // 범위 타일 숨기기
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