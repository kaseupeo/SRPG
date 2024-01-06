using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private Define.GameMode _gameMode;
    private bool _isBeforeStart;
    
    private PlayerCharacter _selectedCharacter;
    private List<PlayerCharacter> _loadPlayerCharacters;
    private List<PlayerCharacter> _playerCharacters;
    private int _maxPlayerCharacter;

    public Define.GameMode GameMode { get => _gameMode; set => _gameMode = value; }
    public bool IsPlacingCharacter { get => _isBeforeStart; set => _isBeforeStart = value; }
    public PlayerCharacter SelectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }
    public List<PlayerCharacter> LoadPlayerCharacters { get => _loadPlayerCharacters; set => _loadPlayerCharacters = value; }
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
    
    // 캐릭터 프리팹 로드 메소드
    private void LoadCharacters()
    {
        _loadPlayerCharacters = new List<PlayerCharacter>();
        
        foreach (GameObject go in Resources.LoadAll("Prefabs/Creatures/PlayerCharacter"))
        {
            _loadPlayerCharacters.Add(go.GetComponent<PlayerCharacter>());
        }
    }
    
    // 캐릭터 생성 메소드
    public void GeneratePlayerCharacter(PlayerCharacter playerCharacter, Tile tile)
    {
        if (!_loadPlayerCharacters.Contains(playerCharacter))
        {
            Debug.Log("오류 : 없는 캐릭터");
            return;
        }

        if (tile == null)
        {
            return;
        }
        
        PlayerCharacter pc = GameObject.Instantiate(playerCharacter.gameObject).GetComponent<PlayerCharacter>();
        pc.Init();
        pc.CharacterPositionOnTile(tile);
        
        _playerCharacters.Add(pc);
    }

    
    
    
    public void MoveAlongPath(PlayerCharacter playerCharacter)
    {
        // playerCharacter.Move();
    }
    
    public void Clear()
    {
        _loadPlayerCharacters.Clear();
    }
    
}