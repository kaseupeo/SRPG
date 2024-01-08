using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateToggleUI : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public Define.State State { get; set; }

    public void SetCharacterState(Define.State state)
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = $"{state}";
        State = state;
    }
}
