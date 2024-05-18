using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;
    public LevelBlock firstBlock;
    public List <LevelBlock> allLevelBlock = new List <LevelBlock> ();
    public List<LevelBlock> currentLevelsBlock = new List <LevelBlock> ();
    public Transform levelInitialPoint;
    public void Awake()
    {
        Instance = this;
        AddNewBlock(true);
        AddNewBlock(false);
    }
    public void AddNewBlock(bool first)
    {
        int randomindex = Random.Range(0, allLevelBlock.Count);
        LevelBlock block;
        if (first) 
        {
            block = (LevelBlock)Instantiate(firstBlock);
        }
        else
        {
            block = (LevelBlock)Instantiate(allLevelBlock[randomindex]);
        }
        
        block.transform.SetParent(transform, false);

        Vector3 blockPosition = Vector3.zero;

        if(currentLevelsBlock.Count == 0)
        {
            blockPosition = levelInitialPoint.position;
        }
        else
        {
            blockPosition = currentLevelsBlock[currentLevelsBlock.Count -1].exitPoint.position;
        }

        block.transform.position = blockPosition;
        currentLevelsBlock.Add(block);
    }

    public void RemoveOldBlock()
    {
        LevelBlock block = currentLevelsBlock[0];
        currentLevelsBlock.Remove(block);
        Destroy(block.gameObject);
    }

    public void RemoveAllBlock()
    {
        while(currentLevelsBlock.Count > 0)
        {
            RemoveOldBlock();
        }
    }
}
