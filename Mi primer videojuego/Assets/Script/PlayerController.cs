using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidbody;

    public float jumpForce = 10f;
    public float runSpeed = 1f;
    public LayerMask groudLayerMask;
    public Animator animator;

    public static PlayerController instance;

    private Vector3 startPosition;
    
    private void Awake()
    {
        startPosition = transform.position;
        instance = this;
        rigidbody = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    public void Start()
    {
        rigidbody.velocity = Vector3.zero;
        animator.SetBool("isAlive", true);
        transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameState == GameStates.inGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            animator.SetBool("isGrounded", IsOnTheFloor());
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.gameState == GameStates.inGame)
        {
            if (rigidbody.velocity.x < runSpeed)
            {
                rigidbody.velocity = new Vector2(runSpeed, rigidbody.velocity.y);
            }
        }
    }

    void Jump()
    {
        if (IsOnTheFloor())
        {
            rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }       
    }

    bool IsOnTheFloor()
    {
       if (Physics2D.Raycast(transform.position, Vector2.down, 1.0f, groudLayerMask.value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void KillPlayer()
    {
        GameManager.instance.GameOver();
        animator.SetBool("isAlive", false);
        rigidbody.velocity = new Vector2(0, 0);
    }
}
