using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Creature
{
    protected int exp;
    protected List<Equipment> equipments;
    protected int currentTurnCost;
    protected int currentAttackCost;
    protected List<Item> items;

    public int Exp { get => exp; set => exp = value; }
    public List<Equipment> Equipments { get => equipments; set => equipments = value; }
    public int CurrentTurnCost { get => currentTurnCost; set => currentTurnCost = value; }
    public int CurrentAttackCost { get => currentAttackCost; set => currentAttackCost = value; }
    public List<Item> Items { get => items; set => items = value; }
    
    public override void Init()
    { 
        base.Init();
        items = new List<Item>();
        ResetTurnCost();
    }

    public void ResetTurnCost()
    {
        currentTurnCost = currentStat.TurnCost;
        currentAttackCost = currentStat.AttackCost;
    }
    
    public override void CharacterPositionOnTile(Tile tile)
    {
        base.CharacterPositionOnTile(tile);
        currentTurnCost--;

        GainItem(tile);
    }

    public override void Attack(Creature target)
    {
        if (currentAttackCost <= 0)
            return;
        
        base.Attack(target);
        currentAttackCost--;
    }

    private void GainItem(Tile tile)
    {
        if (items.Count > 10)
            return;
        
        foreach (var item in Managers.Game.FieldItems)
        {
            if (item.CurrentTile == tile)
            {
                items.Add(item);
            }
        }

        foreach (Item item in items)
        {
            Managers.Game.FieldItems.Remove(item);
            item.CurrentTile = null;
            item.gameObject.SetActive(false);
        }
    }

    // protected override IEnumerator Dead() { }

    private void OnDestroy()
    {
        currentTile.IsBlocked = false;
    }
}