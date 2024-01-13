using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MonoBehaviour
{
    [SerializeField] private ScrollbarUI screenMode;
    [SerializeField] private ScrollbarUI gameSpeedMode;

    private int _i = 0;
    private void OnEnable()
    {
        screenMode.GetComponent<Scrollbar>().value = screenMode.FindValue(Managers.Game.IsFullScreenMode ? 0 : 1);
        gameSpeedMode.GetComponent<Scrollbar>().value = gameSpeedMode.FindValue((int)Managers.Game.GameSpeed / 5);
        Managers.Game.Pause = true;
    }

    private void Update()
    {
        while (_i < 1)
        {
            _i++;
            CheckSettings();
        }
    }

    private void OnDisable()
    {
        Managers.Game.Pause = false;
    }

    public void ResetSettings()
    {
        screenMode.GetComponent<Scrollbar>().value = 0;
        gameSpeedMode.GetComponent<Scrollbar>().value = 0.5f;
    }

    public void SaveSettings()
    {
        Managers.Game.IsFullScreenMode = screenMode.Index == 0;
        Managers.Game.GameSpeed = gameSpeedMode.Index * 5 == 0 ? 0 : gameSpeedMode.Index * 5;
    }

    public void CheckSettings()
    {
        screenMode.GetComponent<Scrollbar>().value = screenMode.FindValue(Managers.Game.IsFullScreenMode ? 0 : 1);
        gameSpeedMode.GetComponent<Scrollbar>().value = gameSpeedMode.FindValue((int)Managers.Game.GameSpeed / 5);
    }
}