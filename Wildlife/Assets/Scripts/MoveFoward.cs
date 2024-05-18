using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFoward : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(transform.position.z < -15 || transform.position.z > 35)
        {
            Destroy(gameObject);
        }
    }
}
