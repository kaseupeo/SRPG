using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private bool _isBeforeStart;
    
    private PlayerCharacter _selectedCharacter;
    private List<PlayerCharacter> _playerCharacterPrefabs;
    private List<PlayerCharacter> _playerCharacters;
    private int _maxPlayerCharacter;

    public bool IsPlacingCharacter { get => _isBeforeStart; set => _isBeforeStart = value; }
    public PlayerCharacter SelectedCharacter { get => _selectedCharacter; set => _selectedCharacter = value; }
    public List<PlayerCharacter> PlayerCharacterPrefabs { get => _playerCharacterPrefabs; set => _playerCharacterPrefabs = value; }
    public List<PlayerCharacter> PlayerCharacters { get => _playerCharacters; set => _playerCharacters = value; }
    public int MaxPlayerCharacter { get => _maxPlayerCharacter; set => _maxPlayerCharacter = value; }
    
    
    public void Init()
    {
        _isBeforeStart = true;
        LoadCharacters();
        _playerCharacters = new List<PlayerCharacter>();
        _maxPlayerCharacter = 2;
    }
    
    private void LoadCharacters()
    {
        _playerCharacterPrefabs = new List<PlayerCharacter>();
        
        foreach (GameObject go in Resources.LoadAll("Prefabs/Creatures/PlayerCharacter"))
        {
            _playerCharacterPrefabs.Add(go.GetComponent<PlayerCharacter>());
        }
    }
    
    public void GeneratePlayerCharacter(PlayerCharacter playerCharacter, Tile tile)
    {
        if (!_playerCharacterPrefabs.Contains(playerCharacter))
        {
            Debug.Log("오류 : 없는 캐릭터");
            return;
        }
        
        PlayerCharacter pc = GameObject.Instantiate(playerCharacter.gameObject).GetComponent<PlayerCharacter>();
        pc.Init();
        pc.CharacterPositionOnTile(tile);
        
        _playerCharacters.Add(pc);
    }

    
    
    public PlayerCharacter SelectedPlayerCharacter(Tile selectTile)
    {
        foreach (PlayerCharacter playerCharacter in _playerCharacters)
        {
            if (playerCharacter.CurrentTile == selectTile)
            {
                _selectedCharacter = playerCharacter;
                
                return playerCharacter;
            }
        }

        return null;
    }

    public void MoveAlongPath(PlayerCharacter playerCharacter)
    {
        // playerCharacter.Move();
    }
    
    public void Clear()
    {
        _playerCharacterPrefabs.Clear();
    }
    
}