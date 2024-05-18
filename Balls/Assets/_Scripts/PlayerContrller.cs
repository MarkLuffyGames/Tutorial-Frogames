using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerContrller : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    float force;
    [SerializeField]
    bool powerUp = false;
    [SerializeField]
    float powerUpTime = 7;
    [SerializeField]
    GameObject[] powerUpIndicator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        rb.AddForce(Vector3.forward * force * inputY, ForceMode.Force);
        rb.AddForce(Vector3.right * force * inputX, ForceMode.Force);

        foreach (GameObject item in powerUpIndicator)
        {
            item.transform.position = transform.position;
        }

        if(transform.position.y < -10)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Power Up"))
        {
            StartCoroutine("PowerUp");
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && powerUp)
        {
            print("collisionm");
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            enemyRigidbody.AddForce((collision.gameObject.transform.position - transform.position)*2000, ForceMode.Force);
        }
    }

    IEnumerator PowerUp()
    {
        powerUp = true;
        foreach (GameObject item in powerUpIndicator)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(powerUpTime / powerUpIndicator.Length);
            item.SetActive(false);
        }
        
        powerUp= false;
    }
}
