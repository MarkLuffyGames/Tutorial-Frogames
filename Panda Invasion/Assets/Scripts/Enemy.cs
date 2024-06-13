using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float healt;
    private float speed;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void RecibeDamage(float damage)
    {
        healt -= damage;

        if(healt <= 0)
        {
            healt = 0;
            _animator.SetTrigger("Dead");
        }
        else
        {
            _animator.SetTrigger("Hit");
        }
    }
}
