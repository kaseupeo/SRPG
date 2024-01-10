using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : NonPlayerCharacter
{
    protected int dropExp;
    [SerializeField]
    protected List<Item> dropItems;
    
    public int DropExp { get => dropExp; set => dropExp = value; }
    public List<Item> DropItems { get => dropItems; set => dropItems = value; }

    public override void Init()
    {
        base.Init();
    }

    protected override IEnumerator Dead()
    {
        StartCoroutine(base.Dead());
        
        for (var i = 0; i < 64; i++)
        {
            yield return null;
        }

        currentTile.IsBlocked = false;
        Drop();
        Destroy(gameObject);
    }

    private void Drop()
    {
        Item dropItem = RandomDropItem();
        Instantiate(dropItem.gameObject, transform.position, Quaternion.identity);
        dropItem.CurrentTile = currentTile;
        Managers.Game.FieldItems.Add(dropItem);
        foreach (PlayerCharacter playerCharacter in Managers.Game.PlayerCharacters)
        {
            playerCharacter.Exp += dropExp;
        }
    }

    private Item RandomDropItem()
    {
        return dropItems[Random.Range(0, dropItems.Count)];
    }
}