using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static bool playerCreated;
    private float speed = 5;
    private Vector2 move;
    private Controls inputPlayer;
    private Animator _animator;
    private Rigidbody2D _rb;
    private Vector2 lastMove;
    private bool walking;
    private bool canMove = true;
    private bool canAttack = true;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject virtualCamera;

    private void Awake()
    {
        if (!playerCreated)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(mainCamera);
            DontDestroyOnLoad(virtualCamera);
            playerCreated = true;
        }
        else
        {
            Destroy(gameObject);
            Destroy(mainCamera);
            Destroy(virtualCamera);
        }

        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        lastMove = Vector2.down;
        
        inputPlayer = new Controls();
        inputPlayer.Enable();

        inputPlayer.Player.Attack.started += Attack_started;

    }

    private void Attack_started(InputAction.CallbackContext obj)
    {
        if (!canAttack) return;
        canMove = false;
        canAttack = false;
        _rb.velocity = Vector2.zero;
        _animator.SetTrigger("Attack");
    }

    private void FixedUpdate()
    {
        if(canMove) Move();
    }

    private void Move()
    {
        move = inputPlayer.Player.Move.ReadValue<Vector2>().normalized;

        if (move != Vector2.zero)
        {

            if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
            {
                SetHorizontal();
            }
            else if (Mathf.Abs(move.x) < Mathf.Abs(move.y))
            {
                SetVertical();
            }
            else
            {
                if (Mathf.Abs(lastMove.x) > Mathf.Abs(lastMove.y))
                {
                    SetHorizontal();
                }
                else
                {
                    SetVertical();
                }
            }

            walking = true;
        }
        else
        {
            walking = false;

            _animator.SetFloat("Vertical", 0);
            _animator.SetFloat("Horizontal", 0);
        }

        
        
        _animator.SetFloat("LastH", lastMove.x);
        _animator.SetFloat("LastV", lastMove.y);
        _animator.SetBool("Walking", walking);


        _rb.velocity = move * speed;
    }

    private void SetHorizontal()
    {
        _animator.SetFloat("Horizontal", new Vector2(move.x, 0).normalized.x);
        _animator.SetFloat("Vertical", 0);
        lastMove = new Vector2(move.x, 0).normalized;
    }

    private void SetVertical()
    {
        _animator.SetFloat("Vertical", new Vector2(move.y, 0).normalized.x);
        _animator.SetFloat("Horizontal", 0);
        lastMove = new Vector2(0, move.y).normalized;
    }

    public void SetNewPosition(Vector3 position)
    {
        transform.position = position;
        virtualCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
        virtualCamera.transform.position = position + Vector3.back * 10;
        virtualCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;
    }

    public Controls GetInputPlayer()
    {
        return inputPlayer;
    }

    public void ActiveMove()
    {
        canMove = true;
        canAttack = true;
    }
}
