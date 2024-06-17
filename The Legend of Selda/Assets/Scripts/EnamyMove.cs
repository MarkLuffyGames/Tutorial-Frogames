using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class EnamyMove : MonoBehaviour
{
    public float speed;
    public float moveTime;
    public float moveTimeCounter;
    public float moveDelay;
    public float moveDelayCounter;
    public bool isMove;
    public Vector2 moveDirection;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        moveTimeCounter = moveTime;
        moveDelayCounter = moveDelay;

        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.gravityScale = 0;
    }
    private void Update()
    {
        if (isMove)
        {
            moveTimeCounter -= Time.deltaTime;
            _rigidbody2D.velocity = moveDirection * speed;

            if(moveTimeCounter <= 0)
            {
                isMove = false;
                _rigidbody2D.velocity = Vector2.zero;
                moveTimeCounter = moveTime * Random.Range(0.5f, 1.6f);
            }
        }
        else
        {
            moveDelayCounter -= Time.deltaTime;

            if(moveDelayCounter <= 0)
            {
                isMove = true;
                moveDirection = new Vector2(Random.Range(-1,2), Random.Range(-1,2));

                moveDelayCounter = moveDelay * Random.Range(0.5f, 1.6f);
            }
        }
    }
}
