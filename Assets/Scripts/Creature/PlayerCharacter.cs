using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Creature
{
    protected int exp;
    protected List<Equipment> equipments;

    public int Exp { get => exp; set => exp = value; }
    public List<Equipment> Equipments { get => equipments; set => equipments = value; }

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
    }

    public override void Init()
    {
        // 임시
        name = "character";
        moveSpeed = 10;
        gameObject.name = name;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    
        base.Init();
    }

    public override void Move(Tile targetTile)
    {
        float step = moveSpeed * Time.deltaTime;
        float zIndex = targetTile.transform.position.z;

        transform.position = Vector2.MoveTowards(transform.position, targetTile.transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);
    }

    public void CharacterPositionOnTile(Tile tile)
    {
        transform.position =
            new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        _spriteRenderer.sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        currentTile = tile;
    }
    public override void Dead() { }
}