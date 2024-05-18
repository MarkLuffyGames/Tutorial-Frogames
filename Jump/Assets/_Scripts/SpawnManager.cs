using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] obstaclePrefabs;
    Vector3 spawnPosition;
    PlayerController _playerController;
    private void Awake()
    {
        spawnPosition = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("Spawn", 0, Random.Range(1f,3f));
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.GameOver)
        {
            CancelInvoke("Spawn");
        }
    }
    
    void Spawn()
    {
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Instantiate(obstaclePrefab, spawnPosition, obstaclePrefab.transform.rotation);
    }
}
