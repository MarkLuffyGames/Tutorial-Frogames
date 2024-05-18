using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Game Over Line"))
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0;
        }
        else
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
