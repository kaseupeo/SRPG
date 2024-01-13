using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanelUI : MonoBehaviour
{
    [SerializeField] private InfoPanelUI playerCharacterInfo;
    [SerializeField] private InfoPanelUI monsterInfo;

    [SerializeField] private Button finishedPlayerTurnButton;

    [SerializeField] private ClearPanelUI clearPanelUI;

    private void Start()
    {
        finishedPlayerTurnButton.onClick.AddListener(FinishedPlayerTurn);
    }

    private void Update()
    {
        playerCharacterInfo.UpdateInfo(Managers.Game.SelectedCharacter);
        monsterInfo.UpdateInfo(Managers.Game.Monster);
    }

    private void FinishedPlayerTurn()
    {
        if (Managers.Game.PlayerCharacters.Count == 0 || Managers.Game.Monsters.Count == 0)
        {
            clearPanelUI.gameObject.SetActive(true);
            return;
        }
        
        
        foreach (PlayerCharacter playerCharacter in Managers.Game.PlayerCharacters) 
            playerCharacter.State = Define.State.Idle;
        
        Managers.Game.GameMode = Define.GameMode.MonsterTurn;
        StartCoroutine(Managers.Game.CoMovePath(Managers.Game.Monsters, Managers.Game.PlayerCharacters));
    }
    
}
