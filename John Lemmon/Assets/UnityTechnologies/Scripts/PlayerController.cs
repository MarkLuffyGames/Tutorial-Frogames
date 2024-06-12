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

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movememt = new Vector3(horizontal, 0, vertical);
        movememt.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0);

        bool isWalking = hasHorizontalInput || hasVerticalInput;
        _animator.SetBool("IsWalking",isWalking);

        Vector3 desiredFoward = Vector3.RotateTowards(transform.forward, movememt, turnSpeed * Time.fixedDeltaTime, 0);

        rotation = Quaternion.LookRotation(desiredFoward);
        
        if(isWalking)
        {
            if (!_audioSource.isPlaying)
            {

                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }

    private void OnAnimatorMove()
    {
        _rb.MovePosition(_rb.position + movememt * _animator.deltaPosition.magnitude);
        _rb.MoveRotation(rotation);
    }
}
