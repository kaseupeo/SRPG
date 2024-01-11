using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparationPanelUI : MonoBehaviour
{
    [SerializeField] private InfoPanelUI playerCharacterInfo;

    private void Update()
    {
        playerCharacterInfo.UpdateInfo(Managers.Game.SelectedCharacter);
    }
}
