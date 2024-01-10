public abstract class NonPlayerCharacter : Creature
{
    protected Creature target;
    
    public Creature Target { get => target; set => target = value; }

    public override void Init()
    {
        base.Init();
    }

    // public abstract override void Dead();
}