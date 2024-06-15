using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float healt;
    [SerializeField] private float speed;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private bool isDead;
    [SerializeField]private int currentWayPoint;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    public void RecibeDamage(float damage)
    {
        healt -= damage;

        if(healt <= 0)
        {
            healt = 0;
            _boxCollider.enabled = false;
            isDead = true;
            _animator.SetTrigger("Dead");
        }
        else
        {
            _animator.SetTrigger("Hit");
        }
    }

    private void Update()
    {
        if (currentWayPoint == GameManager.Instance.wayPoints.Length)
        {
            Eat();
            return;
        }

        float distance = Vector2.Distance(transform.position,
            GameManager.Instance.wayPoints[currentWayPoint].transform.position);

        if(distance <= 0.1)
        {
            currentWayPoint++;
        }
        else
        {
            if(!isDead)Move(GameManager.Instance.wayPoints[currentWayPoint].transform.position);
        }
    }

    private void Move(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    private void Eat()
    {
        _animator.SetTrigger("Eat");
    }

    private void OnDead()
    {
        Destroy(gameObject);
    }
}
