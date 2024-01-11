using UnityEngine;

public class Item : MonoBehaviour
{
    protected int id;
    [SerializeField]
    protected string name;
    protected Define.ItemType itemType;
    protected Tile currentTile;
    

    public int ID { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public Define.ItemType ItemType { get => itemType; set => itemType = value; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }

    public void Init()
    {
        
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }
}