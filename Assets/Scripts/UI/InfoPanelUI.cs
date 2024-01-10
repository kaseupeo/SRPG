using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;

    private PlayerCharacter _playerCharacter;
    private int _level;

    private void Update()
    {
        _playerCharacter = Managers.Game.SelectedCharacter;
        _level = _playerCharacter.Level;
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        image = image;
        text1.text = $"이름 : {_playerCharacter.Name}\n이동력 : {_playerCharacter.Stats[_level].TurnCost}\n공격횟수 : {_playerCharacter.Stats[_level].AttackCost}";
        text2.text = $"체력 : {_playerCharacter.Stats[_level].HealthPoint}\n공격력 : {_playerCharacter.Stats[_level].Attack}\n방어력 : {_playerCharacter.Stats[_level].Defence}";
    }
}