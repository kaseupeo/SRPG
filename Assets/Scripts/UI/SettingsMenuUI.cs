using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuUI : MonoBehaviour
{
    private void OnEnable()
    {
        Managers.Game.Pause = true;
    }

    private void OnDisable()
    {
        Managers.Game.Pause = false;
    }
}
