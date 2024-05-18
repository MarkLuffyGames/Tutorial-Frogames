using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveBlockTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (LevelGenerator.Instance.currentLevelsBlock.Count > 2)
            {
                LevelGenerator.Instance.RemoveOldBlock();
            }
            LevelGenerator.Instance.AddNewBlock(false);
        }
    }
}
