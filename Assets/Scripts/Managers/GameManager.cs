﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager
{
    private Define.GameMode _gameMode;
    
    private PlayerCharacter _selectedCharacter;
    private Monster _monster;
    private List<PlayerCharacter> _loadPlayerCharacters;
    private List<Monster> _loadMonsters;
    private List<PlayerCharacter> _playerCharacters;
    private List<Monster> _monsters;
    private int _maxPlayerCharacter;
    
    public Define.GameMode GameMode { get => _gameMode; set => _gameMode = value; }
    public PlayerCharacter SelectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }
    public Monster Monster { get => _monster; set => _monster = value; }
    public List<PlayerCharacter> LoadPlayerCharacters { get => _loadPlayerCharacters; set => _loadPlayerCharacters = value; }
    public List<Monster> LoadMonsters { get => _loadMonsters; set => _loadMonsters = value; }
    public List<PlayerCharacter> PlayerCharacters { get => _playerCharacters; set => _playerCharacters = value; }
    public List<Monster> Monsters { get => _monsters; set => _monsters = value; }
    public int MaxPlayerCharacter { get => _maxPlayerCharacter; set => _maxPlayerCharacter = value; }
    
    
    public void Init()
    {
        _gameMode = Define.GameMode.Preparation;
        LoadCharacters();
        _playerCharacters = new List<PlayerCharacter>();
        _monsters = new List<Monster>();
        _maxPlayerCharacter = 3;
    }
    
    // 플레이어캐릭터 & 몬스터 프리팹 로드 메소드
    private void LoadCharacters()
    {
        _loadPlayerCharacters = new List<PlayerCharacter>();
        _loadMonsters = new List<Monster>();
        foreach (GameObject pc in Resources.LoadAll("Prefabs/Creatures/PlayerCharacter"))
        {
            _loadPlayerCharacters.Add(pc.GetComponent<PlayerCharacter>());
        }

        foreach (GameObject monster in Resources.LoadAll("Prefabs/Creatures/Monster"))
        {
            _loadMonsters.Add(monster.GetComponent<Monster>());
        }
    }
    
    // 캐릭터 생성 메소드
    public void GeneratePlayerCharacter(Tile tile)
    {
        if (!_loadPlayerCharacters.Contains(_selectedCharacter) || tile == null || tile.IsBlocked)
            return;
        
        PlayerCharacter find = _playerCharacters.Find(x => x.Name == _selectedCharacter.Name);
        if (find != null)
        {
            var gameObject = GameObject.Find(find.Name);
            _playerCharacters.Remove(gameObject.GetComponent<PlayerCharacter>());
            // Debug.Log($"{gameObject}");
            GameObject.Destroy(gameObject);
        }
        
        if (_maxPlayerCharacter <= _playerCharacters.Count)
        {
            // _gameMode = Define.GameMode.Battle;
            return;
        }
        
        PlayerCharacter pc = GameObject.Instantiate(_selectedCharacter.gameObject).GetComponent<PlayerCharacter>();
        pc.Init();
        pc.CharacterPositionOnTile(tile);
        
        _playerCharacters.Add(pc);
    }

    // 몬스터 랜덤 위치에 생성
    public void GenerateRandomMonster()
    {
        List<Tile> tiles = new List<Tile>(Managers.Map.UpdateMapTiles.Values);
        
        foreach (Monster loadMonster in _loadMonsters)
        {
            Monster monster = GameObject.Instantiate(loadMonster.gameObject).GetComponent<Monster>();
            monster.Init();
            monster.CharacterPositionOnTile(tiles[Random.Range(0, tiles.Count)]);
            _monsters.Add(monster);
        }
    }
    
    // 최소 ~ 최대 범위 타일 리스트
    public List<Tile> GetRangeTiles(Tile startTile, int maxRange, int minRange = 0)
    {
        if (startTile == null)
            return null;

        List<Tile> rangeFindingTiles = GetTilesInRange(startTile.Grid2DLocation, maxRange);

        foreach (Tile tile in GetTilesInRange(startTile.Grid2DLocation, minRange))
            rangeFindingTiles.Remove(tile);
        
        rangeFindingTiles.Add(startTile);

        return rangeFindingTiles;
    }

    private List<Tile> GetTilesInRange(Vector2Int location, int range)
    {
        Tile startTile = Managers.Map.MapTiles[location];
        List<Tile> inRangeTile = new List<Tile>();
        List<Tile> tilesForPreviousStep = new List<Tile>();
        int stepCount = 0;
        
        tilesForPreviousStep.Add(startTile);

        while (stepCount < range)
        {
            List<Tile> surroundingTiles = new List<Tile>();
            
            foreach (Tile tile in tilesForPreviousStep)
                surroundingTiles.AddRange(PathFinding.GetSurroundingTiles(new Vector2Int(tile.GridLocation.x, tile.GridLocation.y), Managers.Map.MapTiles));
            
            inRangeTile.AddRange(surroundingTiles);
            tilesForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTile.Distinct().ToList();
    }

    // 1. 몬스터의 공격 범위 만큼 플레이어 중심으로 타일 찾기
    // 찾은 타일들에서 몬스터까지 길찾기해서 가장 잛은 경로 저장
    // -> 몬스터까지 거리가 된다면 그곳으로
    // -> 되지 않는다면 다시 길찾기해서 가장 가까운곳으로
    private List<Tile> MonsterMovement(Creature monster, List<PlayerCharacter> targets)
    {
        List<Tile> shortestTiles = PathFinding.FindPath(monster.CurrentTile,
            targets[Random.Range(0, targets.Count)].CurrentTile, new List<Tile>());
        List<Tile> attackRangeTiles = new List<Tile>();
        
        foreach (PlayerCharacter playerCharacter in targets)
        {
            attackRangeTiles = GetRangeTiles(playerCharacter.CurrentTile, monster.Stats[monster.Level].MaxAttackRange, monster.Stats[monster.Level].MinAttackRange);
            
            foreach (Tile attackRangeTile in attackRangeTiles)
            {
                
                List<Tile> path = PathFinding.FindPath(monster.CurrentTile, attackRangeTile, new List<Tile>());

                if (attackRangeTile == playerCharacter.CurrentTile)
                    continue;

                if (shortestTiles.Count == 0)
                {
                    shortestTiles = path;
                }

                if (shortestTiles.Count > path.Count && path.Count <= monster.Stats[monster.Level].TurnCost)
                {
                    shortestTiles = path;
                }
            }
        }
        
        return shortestTiles;
    }
    
    public IEnumerator CoMovePath(List<Monster> monsters, List<PlayerCharacter> targets)
    {
        Debug.Log("CoMovePath");
        foreach (Monster monster in monsters)
        {
            Debug.Log($"{monster.name} move");
            _monster = monster;
            monster.State = Define.State.Move;
            List<Tile> path = MonsterMovement(monster, targets);
            int cost = monster.Stats[monster.Level].TurnCost;
            while (true)
            {
                yield return null;
                
                if (path.Count <= 0 || cost == 0)
                {
                    List<Tile> attackRangeTiles = PathFinding.GetAttackSurroundingTiles(monster.CurrentTile.Grid2DLocation, Managers.Map.MapTiles, monster.Stats[monster.Level].MaxAttackRange);
                    
                    monster.State = Define.State.Idle;
                    foreach (Tile tile in attackRangeTiles)
                    {
                        foreach (PlayerCharacter target in _playerCharacters)
                        {
                            Debug.Log($"{target.name} takedamege {target.CurrentTile.name}");
                            Debug.Log($"{tile.name}");
                            if (tile == target.CurrentTile)
                            {
                                Debug.Log($"{monster.name} attack");
                                monster.State = Define.State.Attack;
                                monster.Attack(target);
                                monster.State = Define.State.Idle;
                                break;
                            }
                        }
                    }
                    
                    Debug.Log($"{monster.name} idle");
                    
                    monster.State = Define.State.Idle;
                    Managers.Game.ResetTurn();
                    break;
                }

                monster.Move(path[0]);
                
                if (Vector2.Distance(monster.transform.position, path[0].transform.position) < 0.00001f)
                {
                    monster.CharacterPositionOnTile(path[0]);
                    path.RemoveAt(0);
                    cost--;
                }
            }

            _monster = null;
        }
        
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
    }

    
    public void ResetTurn()
    {
        foreach (PlayerCharacter playerCharacter in _playerCharacters)
        {
            playerCharacter.ResetTurnCost();
        }
    }
    
    public void Clear()
    {
        _loadPlayerCharacters.Clear();
    }
}


    // private List<Tile> MonsterMovement(Creature monster, Creature target)
    // {
    //     _monsterState = Define.State.Move;
    //     
    //     // 1. 몬스터의 공격 범위만큼 타겟 중심으로 타일 찾기
    //     List<Tile> attackRangeTiles = GetRangeTiles(target.CurrentTile, monster.Stats[monster.Level].MaxAttackRange);
    //
    //     // List<Tile> monsterMoveTiles = GetRangeTiles(monster.CurrentTile, 1000/* monster.Stats[monster.Level].TurnCost*/);
    //
    //     List<Tile> shortestTiles = new List<Tile>();
    //     foreach (Tile attackRangeTile in attackRangeTiles)
    //     {
    //         if (attackRangeTile == target.CurrentTile)
    //             continue;
    //         
    //         List<Tile> path = PathFinding.FindPath(monster.CurrentTile, attackRangeTile, new List<Tile>());
    //
    //         if (shortestTiles.Count == 0)
    //         {
    //             shortestTiles = path;
    //         }
    //
    //         if (shortestTiles.Count > path.Count && path.Count <= monster.Stats[monster.Level].TurnCost)
    //         {
    //             shortestTiles = path;
    //         }
    //     }
    //
    //     return shortestTiles;
    // }
    //
    // public IEnumerator CoMovePath(Creature creature, Creature target)
    // {
    //     List<Tile> path = MonsterMovement(creature, target);
    //     int cost = creature.Stats[creature.Level].TurnCost;
    //     while (true)
    //     {
    //         yield return null;
    //         
    //         if (path.Count <= 0 || cost == 0)
    //         {
    //             _monsterState = Define.State.Attack;
    //             List<Tile> attackRangeTiles = GetRangeTiles(creature.CurrentTile, creature.Stats[creature.Level].MaxAttackRange, creature.Stats[creature.Level].MinAttackRange, false);
    //
    //             foreach (Tile tile in attackRangeTiles)
    //             {
    //                 if (tile == target.CurrentTile)
    //                 {
    //                     creature.Attack(target.CurrentTile);
    //                     break;
    //                 }
    //             }
    //
    //             _monsterState = Define.State.Idle;
    //             Managers.Game.GameMode = Define.GameMode.PlayerTurn;
    //             Managers.Game.ResetTurn();
    //             yield break;
    //         }
    //
    //         creature.Move(path[0]);
    //         
    //         if (Vector2.Distance(creature.transform.position, path[0].transform.position) < 0.00001f)
    //         {
    //             creature.CharacterPositionOnTile(path[0]);
    //             path.RemoveAt(0);
    //             cost--;
    //         }
    //     }
    // }