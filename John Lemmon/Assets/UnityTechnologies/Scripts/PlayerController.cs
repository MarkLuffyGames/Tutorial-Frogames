using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float turnSpeed;
    Vector3 movememt;
    Quaternion rotation = Quaternion.identity;
    Animator _animator;
    Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movememt = new Vector3(horizontal, 0, vertical);
        movememt.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        bool isWalking = hasHorizontalInput || hasVerticalInput;
        _animator.SetBool("IsWalking",isWalking);

        Vector3 desiredFoward = Vector3.RotateTowards(transform.forward, movememt, turnSpeed * Time.deltaTime, 0);

        rotation = Quaternion.LookRotation(desiredFoward);
        
    }

    private void OnAnimatorMove()
    {
        _rb.MovePosition(_rb.position + movememt * _animator.deltaPosition.magnitude);
        _rb.MoveRotation(rotation);
    }
}
