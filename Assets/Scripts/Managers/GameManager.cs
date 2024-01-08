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
    private int _maxPlayerCharacter;
    
    public Define.GameMode GameMode { get => _gameMode; set => _gameMode = value; }
    public Define.State State { get => _state; set => _state = value; }
    public bool IsPlacingCharacter { get => _isBeforeStart; set => _isBeforeStart = value; }
    public PlayerCharacter SelectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }
    public List<PlayerCharacter> LoadPlayerCharacters { get => _loadPlayerCharacters; set => _loadPlayerCharacters = value; }
    public List<Monster> LoadMonsters { get => _loadMonsters; set => _loadMonsters = value; }
    public List<PlayerCharacter> PlayerCharacters { get => _playerCharacters; set => _playerCharacters = value; }
    public int MaxPlayerCharacter { get => _maxPlayerCharacter; set => _maxPlayerCharacter = value; }
    
    
    public void Init()
    {
        _gameMode = Define.GameMode.Preparation;
        _isBeforeStart = true;
        LoadCharacters();
        _playerCharacters = new List<PlayerCharacter>();
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