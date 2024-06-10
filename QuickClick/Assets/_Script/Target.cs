using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Target : MonoBehaviour
{
    Rigidbody _rigidbody;
    [SerializeField]
    float minForce = 10, maxForce = 12;
    [SerializeField]
    float torque = 100;
    [SerializeField]
    int scoreToAdd = 5;
    [SerializeField]
    ParticleSystem explotionParticle;

    GameManager gameManager;

    public bool isGood;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody= GetComponent<Rigidbody>();
        _rigidbody.AddForce(Vector3.up * RandomForce(), ForceMode.Impulse);
        _rigidbody.AddTorque(RandomVector3());
        gameManager = FindObjectOfType<GameManager>();
    }

    float RandomForce()
    {
        return Random.Range(minForce, maxForce);
    }

    Vector3 RandomVector3()
    {
        return new Vector3
            (Random.Range(-torque, torque),
            Random.Range(-torque, torque), 
            Random.Range(-torque, torque));
    }

    private void OnMouseDown()
    {
        if (!gameManager.gameOver && isGood)
        {
            gameManager.UpdateScore(scoreToAdd);
            StartExplotion();
        }
        else if(!gameManager.gameOver && !isGood)
        {
            gameManager.GameOver();
            StartExplotion();
        }
        
    }

    void StartExplotion()
    {
        Instantiate(explotionParticle, transform.position, explotionParticle.transform.rotation);
        Destroy(gameObject);
    }
}
