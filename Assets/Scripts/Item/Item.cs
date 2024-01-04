using UnityEngine;

public class Item : MonoBehaviour
{
    protected int id;
    protected string name;
    protected Define.ItemType itemType;

    public int ID { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public Define.ItemType ItemType { get => itemType; set => itemType = value; }

    public void Init()
    {
        
    }
}