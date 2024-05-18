using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    GameObject player;
    Rigidbody rb;
    [SerializeField]
    float force = 500;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        direction = (player.transform.position - transform.position).normalized;
        rb.AddForce(direction * force, ForceMode.Force);

        if(transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
