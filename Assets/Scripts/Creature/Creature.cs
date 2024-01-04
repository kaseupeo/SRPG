using System;
using UnityEngine;
using UnityEngine.Serialization;

// TODO
public abstract class Creature : MonoBehaviour
{
    protected string name;
    protected Stat stat;
    protected Define.CreatureState state;
    protected Tile currentTile;
    
    public string Name { get => name; set => name = value; }
    public Stat Stat { get => stat; set => stat = value; }
    public Define.CreatureState State { get => state; set => state = value; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }

    public void Init()
    {
        state = Define.CreatureState.Idle;
    }

    public abstract void Move();
    public abstract void Dead();

}

[Serializable]
public class Stat
{
    [SerializeField] private int level;
    [SerializeField] private int healthPoint;
    [SerializeField] private int attack;
    [SerializeField] private int block;
    [SerializeField] private int contact;
    [SerializeField] private int defence;
    [SerializeField] private int speed;
    [SerializeField] private int luck;

    public int Level { get => level; set => level = value; }
    public int HealthPoint { get => healthPoint; set => healthPoint = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Block { get => block; set => block = value; }
    public int Contact { get => contact; set => contact = value; }
    public int Defence { get => defence; set => defence = value; }
    public int Speed { get => speed; set => speed = value; }
    public int Luck { get => luck; set => luck = value; }
}