﻿using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    protected int id;
    [SerializeField] protected string name;
    [SerializeField] protected Sprite sprite;
    protected Define.ItemType itemType;
    protected Tile currentTile;
    

    public int ID { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public Define.ItemType ItemType { get => itemType; set => itemType = value; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }

    public void Init()
    {
        
    }

    private void OnDestroy()
    {
        currentTile = null;
    }
}