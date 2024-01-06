using TMPro;
using UnityEngine;

public class CharacterToggleUI : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public void SetPlayerCharacter(PlayerCharacter playerCharacter)
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = $"{playerCharacter.Name}";
    }
}