using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Creature
{
    protected int exp;
    protected List<Equipment> equipments;
    protected int currentTurnCost;
    protected int currentAttackCost;

    public int Exp { get => exp; set => exp = value; }
    public List<Equipment> Equipments { get => equipments; set => equipments = value; }
    public int CurrentTurnCost { get => currentTurnCost; set => currentTurnCost = value; }
    public int CurrentAttackCost { get => currentAttackCost; set => currentAttackCost = value; }

    public override void Init()
    { 
        ResetTurnCost();
    
        base.Init();
    }

    public void ResetTurnCost()
    {
        currentTurnCost = stats[level].TurnCost;
        currentAttackCost = stats[level].AttackCost;
    }
    
    public override void CharacterPositionOnTile(Tile tile)
    {
        base.CharacterPositionOnTile(tile);
        currentTurnCost--;
    }

    public override void Attack(Tile targetTile)
    {
        if (currentAttackCost <= 0)
            return;
        
        base.Attack(targetTile);
        currentAttackCost--;
    }
    
    public override void Dead() { }

    private void OnDestroy()
    {
        currentTile.IsBlocked = false;
    }
}