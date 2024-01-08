﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Creature
{
    protected int exp;
    protected List<Equipment> equipments;
    protected int currentTurnCost;

    public int Exp { get => exp; set => exp = value; }
    public List<Equipment> Equipments { get => equipments; set => equipments = value; }
    public int CurrentTurnCost { get => currentTurnCost; set => currentTurnCost = value; }


    public override void Init()
    {
        // 임시
        moveSpeed = 1;
        gameObject.name = name;
        ResetTurnCost();
    
        base.Init();
    }

    public void ResetTurnCost()
    {
        currentTurnCost = stats[level].TurnCost;
    }
    
    public override void CharacterPositionOnTile(Tile tile)
    {
        base.CharacterPositionOnTile(tile);
        currentTurnCost--;
    }
    
    public override void Dead() { }

    private void OnDestroy()
    {
        currentTile.IsBlocked = false;
    }
}