using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// TODO
public abstract class Creature : MonoBehaviour
{
    [SerializeField]
    protected string name;
    protected int level;
    protected float moveSpeed;
    [SerializeField]
    protected List<Stat> statsList;
    protected List<Skill> skills;
    protected Define.CreatureState state;
    protected Tile currentTile;
    
    public string Name { get => name; set => name = value; }
    public int Level { get => level; set => level = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public List<Stat> Stats { get => statsList; set => statsList = value; }
    public List<Skill> Skills { get => skills; set => skills = value; }
    public Define.CreatureState State { get => state; set => state = value; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }
    
    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        state = Define.CreatureState.Idle;
    }

    public abstract void Move(Tile targetTile);
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
    [SerializeField] private int turnSpeed;
    [SerializeField] private int turnCost;

    public int Level { get => level; set => level = value; }
    public int HealthPoint { get => healthPoint; set => healthPoint = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Block { get => block; set => block = value; }
    public int Contact { get => contact; set => contact = value; }
    public int Defence { get => defence; set => defence = value; }
    public int TurnSpeed { get => turnSpeed; set => turnSpeed = value; }
    public int TurnCost { get => turnCost; set => turnCost = value; }
}