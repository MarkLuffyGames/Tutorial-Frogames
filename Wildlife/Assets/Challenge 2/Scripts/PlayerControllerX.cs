using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    float counter = 0f;
    float nextTime = 1f;
    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space) && counter >= nextTime)
        {
            Instantiate(dogPrefab, transform.position, dogPrefab.transform.rotation);
            counter = 0f;
        }
        
        
        
    }
}
