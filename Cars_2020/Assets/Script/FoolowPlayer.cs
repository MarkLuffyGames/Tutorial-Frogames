using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoolowPlayer : MonoBehaviour
{
    public GameObject player;
    public Vector3 offSet = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offSet;
    }
}
