using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Slot[] slots;
    
    private List<Item> _items;

    private void OnEnable()
    {
        _items = new List<Item>();
        _items = Managers.Game.SelectedCharacter.Items;
        
        int i = 0;
        foreach (Slot slot in slots)
        {
            if (i >= _items.Count)
                break;
            
            if (slot.Item == null)
            {
                slot.SetItem(_items[i]);
                i++;
            }
        }
    }

    public void Close()
    {
        
    }

    public void Acquire(Item item)
    {
        
    }
}