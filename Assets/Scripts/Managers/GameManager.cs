using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private Define.GameMode _gameMode;
    private Define.State _state;
    private bool _isBeforeStart;
    
    private PlayerCharacter _selectedCharacter;
    private List<PlayerCharacter> _loadPlayerCharacters;
    private List<Monster> _loadMonsters;
    private List<PlayerCharacter> _playerCharacters;
    private List<Monster> _monsters;
    private int _maxPlayerCharacter;
    
    public Define.GameMode GameMode { get => _gameMode; set => _gameMode = value; }
    public Define.State State { get => _state; set => _state = value; }
    public bool IsPlacingCharacter { get => _isBeforeStart; set => _isBeforeStart = value; }
    public PlayerCharacter SelectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }
    public List<PlayerCharacter> LoadPlayerCharacters { get => _loadPlayerCharacters; set => _loadPlayerCharacters = value; }
    public List<Monster> LoadMonsters { get => _loadMonsters; set => _loadMonsters = value; }
    public List<PlayerCharacter> PlayerCharacters { get => _playerCharacters; set => _playerCharacters = value; }
    public List<Monster> Monsters { get => _monsters; set => _monsters = value; }
    public int MaxPlayerCharacter { get => _maxPlayerCharacter; set => _maxPlayerCharacter = value; }
    
    
    public void Init()
    {
        _gameMode = Define.GameMode.Preparation;
        _isBeforeStart = true;
        LoadCharacters();
        _playerCharacters = new List<PlayerCharacter>();
        _monsters = new List<Monster>();
        _maxPlayerCharacter = 2;
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
    
    public List<Tile> GetRangeTiles(Creature creature, int maxRange, int minRange = 0)
    {
        if (creature == null)
            return null;

        List<Tile> rangeFindingTiles = PathFinding.GetTilesInRange(creature.CurrentTile.Grid2DLocation, maxRange);

        foreach (Tile tile in PathFinding.GetTilesInRange(creature.CurrentTile.Grid2DLocation, minRange))
            rangeFindingTiles.Remove(tile);
        
        return rangeFindingTiles;
    }

    // 몬스터의 공격 범위 만큼 플레이어 중심으로 타일 찾기
    // 찾은 타일들에서 몬스터까지 길찾기해서 가장 잛은 경로 저장
    // -> 몬스터까지 거리가 된다면 그곳으로
    // -> 되지 않는다면 다시 길찾기해서 가장 가까운곳으로
    public void MonsterMovement()
    {
        foreach (Monster monster in _monsters)
        {
            List<Tile> rangeFindingTiles = GetRangeTiles(monster, monster.Stats[monster.Level].TurnCost);
            
            
            
        }
    }

    public IEnumerator CoMovement(Creature creature, List<Tile> path)
    {
        while (true)
        {
            yield return null;
            
            creature.Move(path[0]);

            if (Vector2.Distance(creature.transform.position, path[0].transform.position) < 0.00001f)
            {
                creature.CharacterPositionOnTile(path[0]);
                path.RemoveAt(0);
            }

            if (path.Count == 0)
            {
                
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