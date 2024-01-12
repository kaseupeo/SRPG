using UnityEngine;

public class Consumption : Item
{
    protected int count;
    
    public int Count { get => count; set => count = value; }

    public virtual void Use()
    {
        transform.position = Managers.Game.SelectedCharacter.transform.position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder =
            Managers.Game.SelectedCharacter.GetComponent<SpriteRenderer>().sortingOrder + 1;
        Managers.Game.SelectedCharacter.Items.Remove(this);
        gameObject.SetActive(true);
    }

    public void GetItem()
    {
        
    }
}