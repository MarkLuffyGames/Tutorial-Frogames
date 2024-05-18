using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float rotateSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * speed * Time.deltaTime * inputVertical);
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime * inputHorizontal);
    }

}
