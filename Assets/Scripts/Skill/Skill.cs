using UnityEngine;

public class Skill : MonoBehaviour
{
    protected int id;
    protected string name;
    protected Define.SkillType type;
    protected int power;
    protected int point;

    public int ID { get => id; set => id = value; }
    public string Name { get => name; set => name = value; }
    public Define.SkillType Type { get => type; set => type = value; }
    public int Power { get => power; set => power = value; }
    public int Point { get => point; set => point = value; }
}