public abstract class NonPlayerCharacter : Creature
{
    protected Creature target;
    
    public Creature Target { get => target; set => target = value; }

    public override void Init()
    {
        base.Init();
    }
    
    public override void Move()
    {
        // TODO : 타겟으로 이동 로직 및 공격
    }

    public abstract override void Dead();
}