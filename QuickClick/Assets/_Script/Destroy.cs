using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<Target>();

        if (target != null)
        {
            if(target.isGood)
            {
                if(!gameManager.gameOver) gameManager.UpdateHealt();
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
        
    }
}
