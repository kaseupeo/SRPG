using System;
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
        if (_item != null)
            return;
        
        image.sprite = item.Sprite;
        _item = item;
        image.color = new Color(1, 1, 1, 1);
    }

    public void ResetSlot()
    {
        image.color = new Color(1, 1, 1, 0);
        image.sprite = null;
        _item = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            switch (_item.ItemType)
            {
                case Define.ItemType.Equipment:
                    break;
                case Define.ItemType.Consumption:
                    Consumption consumption = _item as Consumption;
                    consumption.Use();
                    Debug.Log($"{_item}");
                    ResetSlot();
                    Debug.Log($"{_item}");
                    break;
                default:
                    break;
            }
        }
    }
}
