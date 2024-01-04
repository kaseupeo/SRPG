using System.Collections.Generic;

public class PlayerCharacter : Creature
{
    protected int exp;
    protected List<Equipment> equipments;

    public int Exp { get => exp; set => exp = value; }
    public List<Equipment> Equipments { get => equipments; set => equipments = value; }

    public override void Init()
    {
        base.Init();
    }
    
    public override void Move() { }
    public override void Dead() { }
}