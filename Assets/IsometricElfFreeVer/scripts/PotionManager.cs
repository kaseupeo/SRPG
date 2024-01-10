using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private float _speed = 0.2f;
    public float leftTime;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }


    private void Use()
    {
        _animator.SetTrigger("Use");
        _rigidbody2D.velocity = transform.up * _speed;
        Destroy(gameObject, leftTime);
    }
}
