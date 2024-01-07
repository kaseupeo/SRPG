using System;
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

    private SpriteRenderer _spriteRenderer;

    public override void Init()
    {
        // 임시
        moveSpeed = 10;
        gameObject.name = name;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        ResetTurnCost();
    
        base.Init();
    }

    public override void Move(Tile targetTile)
    {
        float step = moveSpeed * Time.deltaTime;
        float zIndex = targetTile.transform.position.z;
        currentTile.IsBlocked = false;
        
        transform.position = Vector2.MoveTowards(transform.position, targetTile.transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);
    }

    public void ResetTurnCost()
    {
        currentTurnCost = stats[level].TurnCost;
    }
    
    public void CharacterPositionOnTile(Tile tile)
    {
        transform.position =
            new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        _spriteRenderer.sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        tile.IsBlocked = true;
        currentTile = tile;
        currentTurnCost--;
    }
    public override void Dead() { }

    private void OnDestroy()
    {
        currentTile.IsBlocked = false;
    }
}