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
    protected List<Stat> stats;
    protected List<Skill> skills;
    protected Define.CreatureState state;
    protected Tile currentTile;
    
    public string Name { get => name; set => name = value; }
    public int Level { get => level; set => level = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public List<Stat> Stats { get => stats; set => stats = value; }
    public List<Skill> Skills { get => skills; set => skills = value; }
    public Define.CreatureState State { get => state; set => state = value; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }
    
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    
    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        state = Define.CreatureState.Idle;
    }
    
    /*
     * 좌하단 방향 : -x, -y
     * 좌상단 방향 : -x, +y
     */
    public void Move(Tile targetTile)
    {
        float step = moveSpeed * Time.deltaTime;
        float zIndex = targetTile.transform.position.z;
        currentTile.IsBlocked = false;
        
        transform.position = Vector2.MoveTowards(transform.position, targetTile.transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        Vector2 dir = targetTile.transform.position - transform.position;
        
        // 이동 애니메이션
        if (dir.x != 0 || dir.y != 0)
        {
            animator.SetFloat("X", dir.x);
            animator.SetFloat("Y", dir.y);
            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
    }
    
    public virtual void CharacterPositionOnTile(Tile tile)
    {
        transform.position =
            new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        spriteRenderer.sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        tile.IsBlocked = true;
        currentTile = tile;
    }
    
    public void Attack()
    {
        animator.SetTrigger("Slash");
        // TODO : 공격
    }
    
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