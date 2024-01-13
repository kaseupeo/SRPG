using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparationPanelUI : MonoBehaviour
{
    [SerializeField] private InfoPanelUI playerCharacterInfo;

    [SerializeField] private Button battleStartButton;

    private void Start()
    {
        battleStartButton.onClick.AddListener(BattleStart);
    }

    private void Update()
    {
        playerCharacterInfo.UpdateInfo(Managers.Game.SelectedCharacter);
    }
    
    public void BattleStart()
    {
        if (Managers.Game.PlayerCharacters.Count == 0)
            return;
        
        Managers.Game.SelectedCharacter = null;
        Managers.Game.GameMode = Define.GameMode.PlayerTurn;
        Managers.Game.GenerateMonster();
    }
}
