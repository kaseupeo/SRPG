using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapDiffToggleUI : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI _text;
    private Toggle _toggle;
    private GameObject _gameObject;

    public void SetMap(GameObject go)
    {
        _toggle = GetComponent<Toggle>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _text.text = $"{go.name}";
        _gameObject = go;

        if (_toggle.isOn) Managers.Game.Map = _gameObject;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Managers.Game.Map = _gameObject;
        }
    }
}