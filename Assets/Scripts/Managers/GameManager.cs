using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    private GameObject _character;
    private List<PlayerCharacter> _playerCharacters;
    public GameObject Character { get => _character; set => _character = value; }
    public List<PlayerCharacter> PlayerCharacters
    {
        get
        {
            if (_playerCharacters == null)
            {
                GenerateCharacters();
            }
            return _playerCharacters;
        }
        set => _playerCharacters = value;
    }

    public void GenerateCharacters()
    {
        _playerCharacters = new List<PlayerCharacter>();
        
        foreach (var obj in Resources.LoadAll("Prefabs/Creatures/PlayerCharacter"))
        {
            var gameObject = (GameObject)obj;
            _playerCharacters.Add(gameObject.GetComponent<PlayerCharacter>());
        }

    }

    public void Clear()
    {
        _playerCharacters.Clear();
    }
    
}