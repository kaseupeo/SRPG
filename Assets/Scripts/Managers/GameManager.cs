using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private Define.GameMode _gameMode;
    private Define.State _monsterState;
    
    private PlayerCharacter _selectedCharacter;
    private List<PlayerCharacter> _loadPlayerCharacters;
    private List<Monster> _loadMonsters;
    private List<PlayerCharacter> _playerCharacters;
    private List<Monster> _monsters;
    private int _maxPlayerCharacter;
    
    public Define.GameMode GameMode { get => _gameMode; set => _gameMode = value; }
    public Define.State MonsterState { get => _monsterState; set => _monsterState = value; }
    public PlayerCharacter SelectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }
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
        _maxPlayerCharacter = 2;
        _monsterState = Define.State.Idle;
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
        List<Tile> tiles = new List<Tile>(Managers.Map.MapTiles.Values);

        foreach (Monster loadMonster in _loadMonsters)
        {
            Monster monster = GameObject.Instantiate(loadMonster.gameObject).GetComponent<Monster>();
            monster.Init();
            monster.CharacterPositionOnTile(tiles[Random.Range(0, tiles.Count)]);
            _monsters.Add(monster);
        }
    }
    
    // 최소 ~ 최대 범위 타일 리스트
    public List<Tile> GetRangeTiles(Tile startTile, int maxRange, int minRange = 0, bool isMoving = true)
    {
        if (startTile == null)
            return null;

        List<Tile> rangeFindingTiles = PathFinding.GetTilesInRange(startTile.Grid2DLocation, maxRange);

        foreach (Tile tile in PathFinding.GetTilesInRange(startTile.Grid2DLocation, minRange))
            rangeFindingTiles.Remove(tile);

        if (isMoving) 
            rangeFindingTiles.Add(startTile);

        return rangeFindingTiles;
    }

    // 1. 몬스터의 공격 범위 만큼 플레이어 중심으로 타일 찾기
    // 찾은 타일들에서 몬스터까지 길찾기해서 가장 잛은 경로 저장
    // -> 몬스터까지 거리가 된다면 그곳으로
    // -> 되지 않는다면 다시 길찾기해서 가장 가까운곳으로
    private List<Tile> MonsterMovement(Creature monster, List<PlayerCharacter> targets, out Creature target)
    {
        List<Tile> shortestTiles = PathFinding.FindPath(monster.CurrentTile,
            targets[Random.Range(0, targets.Count)].CurrentTile, new List<Tile>());
        List<Tile> attackRangeTiles = new List<Tile>();
        target = null;
        
        foreach (PlayerCharacter playerCharacter in targets)
        {
            attackRangeTiles = GetRangeTiles(playerCharacter.CurrentTile, monster.Stats[monster.Level].MaxAttackRange);
            
            foreach (Tile attackRangeTile in attackRangeTiles)
            {
                if (attackRangeTile == playerCharacter.CurrentTile)
                    continue;
            
                List<Tile> path = PathFinding.FindPath(monster.CurrentTile, attackRangeTile, new List<Tile>());

                if (shortestTiles.Count == 0)
                {
                    shortestTiles = path;
                    target = playerCharacter;
                }

                if (shortestTiles.Count > path.Count && path.Count <= monster.Stats[monster.Level].TurnCost)
                {
                    shortestTiles = path;
                    target = playerCharacter;
                }
            }
        }
        
        return shortestTiles;
    }
    
    public IEnumerator CoMovePath(Creature creature, List<PlayerCharacter> targets)
    {
        _monsterState = Define.State.Move;
        List<Tile> path = MonsterMovement(creature, targets, out var target);
        int cost = creature.Stats[creature.Level].TurnCost;
        while (true)
        {
            if (creature.State == Define.State.Attack)
                continue;
            
            yield return null;
            
            if (path.Count <= 0 || cost == 0)
            {
                if (target == null)
                    break;
                
                _monsterState = Define.State.Attack;
                List<Tile> attackRangeTiles = GetRangeTiles(creature.CurrentTile, creature.Stats[creature.Level].MaxAttackRange, creature.Stats[creature.Level].MinAttackRange, false);

                foreach (Tile tile in attackRangeTiles)
                {
                    if (tile == target.CurrentTile)
                    {
                        creature.Attack(target.CurrentTile);
                        break;
                    }
                }

                _monsterState = Define.State.Idle;
                Managers.Game.GameMode = Define.GameMode.PlayerTurn;
                Managers.Game.ResetTurn();
                yield break;
            }

            creature.Move(path[0]);
            
            if (Vector2.Distance(creature.transform.position, path[0].transform.position) < 0.00001f)
            {
                creature.CharacterPositionOnTile(path[0]);
                path.RemoveAt(0);
                cost--;
            }
        }
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