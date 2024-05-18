using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject prefabEnemy;
    [SerializeField]
    float spawnRange = 9;
    [SerializeField]
    int enemyCount = 0;
    [SerializeField]
    int spawnCount = 1;
    [SerializeField]
    GameObject powerUp;
    void Start()
    {
        InsstantiateEnemyWave(spawnCount);
        InsstantiatePowerUp();
    }

    private void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if(enemyCount == 0)
        {
            spawnCount++;
            InsstantiateEnemyWave(spawnCount);
            InsstantiatePowerUp();
        }
    }

    /// <summary>
    /// Genera una pisicion aleatoria en el eje X y Z dejando 0 en el eje Y. 
    /// </summary>
    /// <returns>Vector3 de la posicion aleotoria.</returns>
    Vector3 RandomPosition()
    {
        float posX = Random.Range(-spawnRange, spawnRange);
        float posZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(posX, 0, posZ);
        return randomPos;
    }
    /// <summary>
    /// Genera enemigos aleatorios.
    /// </summary>
    /// <param name="numberOfEnemies"></param>
    void InsstantiateEnemyWave(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(prefabEnemy, RandomPosition(), prefabEnemy.transform.rotation);
        }
        
    }

    void InsstantiatePowerUp()
    {
        Instantiate(powerUp, RandomPosition(), powerUp.transform.rotation);
    }
}
