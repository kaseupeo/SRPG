using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePanelUI : MonoBehaviour
{
    [SerializeField] private InfoPanelUI playerCharacterInfo;
    [SerializeField] private InfoPanelUI monsterInfo;

    private void Update()
    {
        playerCharacterInfo.UpdateInfo(Managers.Game.SelectedCharacter);
        monsterInfo.UpdateInfo(Managers.Game.Monster);
    }
}
