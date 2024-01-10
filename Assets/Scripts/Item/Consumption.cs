public class Consumption : Item
{
    protected int count;
    
    public int Count { get => count; set => count = value; }

    public virtual void Use()
    {
    }

    public void GetItem()
    {
        
    }
}