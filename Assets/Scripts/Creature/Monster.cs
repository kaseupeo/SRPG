using System.Collections.Generic;
using UnityEngine;

public class Monster : NonPlayerCharacter
{
    protected int dropExp;
    protected List<Item> dropItems;
    
    public int DropExp { get => dropExp; set => dropExp = value; }
    public List<Item> DropItems { get => dropItems; set => dropItems = value; }

    public override void Init()
    {
        base.Init();
    }

    public override void Dead()
    {
        Drop();
    }

    public void Drop()
    {
        
    }
}