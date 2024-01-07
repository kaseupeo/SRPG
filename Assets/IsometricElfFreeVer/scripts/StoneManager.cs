using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    private float speed = 2.5f;
   [SerializeField] Transform target;

    public float timer = 1f;

    void Update()// 긆긳긙긃긏긣귩뽞뷭?귏궳댷벍궠궧귡
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        Destroy(gameObject, timer);
    }
    
}

