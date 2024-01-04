using System.Collections.Generic;

public class PlayerCharacter : Creature
{
    protected int level;
    protected int exp;
    protected List<Skill> skills;
    protected List<Equipment> equipments;

    public int Level { get => level; set => level = value; }
    public int Exp { get => exp; set => exp = value; }
    public List<Skill> Skills { get => skills; set => skills = value; }
    public List<Equipment> Equipments { get => equipments; set => equipments = value; }
    
    
    public override void Move() { }
    public override void Dead() { }
}