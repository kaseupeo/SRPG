using System;
using UnityEngine;

public class Potion : Consumption
{
    [SerializeField] private float leftTime;
    
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private float _speed;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _speed = 0.2f;
    }

    public override void Use()
    {
        base.Use();
        _animator.SetTrigger("Use");
        _rigidbody2D.velocity = transform.up * _speed;
        Destroy(gameObject, leftTime);
    }
}