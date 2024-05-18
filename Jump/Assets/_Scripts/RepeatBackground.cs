using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RepeatBackground : MonoBehaviour
{
    Vector3 startPos;
    float width;
    
    
    // Start is called before the first frame update
    void Start()
    {
        startPos= transform.position;
        width = GetComponent<BoxCollider>().size.x / 2;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(startPos.x - transform.position.x > width)
        {
            transform.position = startPos;
        }
    }
}
