using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPanelUI : MonoBehaviour
{
    [SerializeField] private CharacterToggleUI characterTogglePrefab;

    private Button _button;
    private ToggleGroup _toggleGroup;

    private List<PlayerCharacter> _playerCharacters;
    private List<CharacterToggleUI> _characterToggles;


    private void OnEnable()
    {
        _button = GetComponentInChildren<Button>();
        _toggleGroup = GetComponentInChildren<ToggleGroup>();
        _playerCharacters = new List<PlayerCharacter>();
        _characterToggles = new List<CharacterToggleUI>();

        _playerCharacters = Managers.Game.LoadPlayerCharacters;
        
        for (int i = 0; i < _playerCharacters.Count; i++)
        {
            var toggle = Instantiate(characterTogglePrefab, _toggleGroup.transform);
            toggle.SetPlayerCharacter(_playerCharacters[i]);
            _characterToggles.Add(toggle);
            toggle.GetComponent<Toggle>().group = _toggleGroup;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _characterToggles.Count; i++)
        {
            if (_characterToggles[i].GetComponent<Toggle>().isOn)
            {
                Managers.Game.SelectedCharacter = _playerCharacters[i];
                break;
            }
        }
    }

}
