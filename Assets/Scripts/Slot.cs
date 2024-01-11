using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;
    private Item _item;

    public Item Item { get => _item; set => _item = value; }

    public void SetItem(Item item)
    {
        image.sprite = item.Sprite;
        _item = item;
        image.color = new Color(1, 1, 1, 1);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            
        }
    }
}
