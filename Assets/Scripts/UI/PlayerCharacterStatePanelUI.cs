using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerCharacterStatePanelUI : MonoBehaviour
{
    [SerializeField] private Toggle togglePrefab;
    
    private ToggleGroup _toggleGroup;
    private List<StateToggleUI> _toggles;
    
    private void OnEnable()
    {
        _toggleGroup = GetComponent<ToggleGroup>();
        _toggles = new List<StateToggleUI>();
        
        // MOVE
        {
            var toggle = Instantiate(togglePrefab, _toggleGroup.transform).gameObject.AddComponent<StateToggleUI>();
            toggle.SetCharacterState(Define.State.Move);
            _toggles.Add(toggle);
            toggle.GetComponent<Toggle>().group = _toggleGroup;
        }
        // ATTACK
        {
            var toggle = Instantiate(togglePrefab, _toggleGroup.transform).gameObject.AddComponent<StateToggleUI>();
            toggle.SetCharacterState(Define.State.Attack);
            _toggles.Add(toggle);
            toggle.GetComponent<Toggle>().group = _toggleGroup;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _toggles.Count; i++)
        {
            if (_toggles[i].GetComponent<Toggle>().isOn)
            {
                Managers.Game.SelectedCharacter.State = _toggles[i].State;
                break;
            }
        }
    }

    private void OnDisable()
    {
        foreach (StateToggleUI toggle in _toggles)
        {
            Destroy(toggle.gameObject);
        }
    }
}
