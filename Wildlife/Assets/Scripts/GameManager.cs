using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> enemiesPrefabs = new List<GameObject>();
    void Start()
    {
        InvokeRepeating("Spawn", 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        int random = Random.Range(0, enemiesPrefabs.Count);
        GameObject enemy = enemiesPrefabs[random];
        Instantiate(enemy, new Vector3(Random.Range(-15, 15), enemy.transform.position.y, enemy.transform.position.z), enemy.transform.rotation);
    }
}
