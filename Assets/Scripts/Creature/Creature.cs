using System;
using System.Collections;
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
    protected Define.State state;
    protected Tile currentTile;
    
    public string Name { get => name; set => name = value; }
    public int Level { get => level; set => level = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public List<Stat> Stats { get => stats; set => stats = value; }
    public Define.State State { get => state; set => state = value; }
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

        state = Define.State.Idle;
        moveSpeed = 1;
        gameObject.name = name;
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
    
    public virtual void Attack(Creature target)
    {
        // TODO : 공격
        Vector2 dir = target.currentTile.transform.position - transform.position;

        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);

        animator.SetTrigger("Slash");

        int damage = stats[level].Attack - target.Stats[target.level].Defence;

        damage = damage > 0 ? damage : 0;

        target.stats[target.level].HealthPoint -= damage;
        Debug.Log($"{target.name} take damage : {damage}\nremaining hp : {target.stats[level].HealthPoint}");

        if (target.stats[target.level].HealthPoint <= 0)
        {
            StartCoroutine(target.Dead());
        }
    }

    protected virtual IEnumerator Dead()
    {
        state = Define.State.Dead;
        animator.SetTrigger("Dead");
        Vector2 currentPosition = transform.position;
        transform.position = new Vector2(currentPosition.x, currentPosition.y -0.12f);
        
        for (var i = 0; i < 64; i++)
        {
            yield return null;
        }

        transform.position = currentPosition;
    }
}

[Serializable]
public class Stat
{
    [SerializeField] private int level;
    [SerializeField] private int healthPoint;
    [SerializeField] private int attack;
    [SerializeField] private int defence;
    [SerializeField] private int turnCost;
    [SerializeField] private int attackCost;
    [SerializeField] private int minAttackRange;
    [SerializeField] private int maxAttackRange;

    public int Level { get => level; set => level = value; }
    public int HealthPoint { get => healthPoint; set => healthPoint = value; }
    public int Attack { get => attack; set => attack = value; }
    public int Defence { get => defence; set => defence = value; }
    public int TurnCost { get => turnCost; set => turnCost = value; }
    public int AttackCost { get => attackCost; set => attackCost = value; }
    public int MinAttackRange { get => minAttackRange; set => minAttackRange = value; }
    public int MaxAttackRange { get => maxAttackRange; set => maxAttackRange = value; }
}