using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    bool isPlayerInRange;

    public GameEnding gameEnding;
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform == player)
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform == player)
        {
            isPlayerInRange = false;
        }
    }

    private void Update()
    {
        if(isPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;

            RaycastHit hit;
            if(Physics.Raycast(transform.position, direction, out hit))
            {
                if(hit.transform == player)
                {
                    gameEnding.CatchPlayer();
                }
            }
        }
    }
}
