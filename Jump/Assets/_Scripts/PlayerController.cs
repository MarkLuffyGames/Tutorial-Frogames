using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody playerRb;
    [SerializeField]
    ParticleSystem dirt;

    [SerializeField]
    float jumpForce;
    [SerializeField]
    float gravityMult = 2;

    bool isGrounder = true;
    private bool _gameOver = false;
    public bool GameOver { get => _gameOver; }


    Animator animator;
    public AudioClip jumpClip, crashClip;
    AudioSource audioSource;


    private void Awake()
    {
        Physics.gravity *= gravityMult;
        playerRb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        animator.SetFloat("Speed_f", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounder && !_gameOver)
        {
            audioSource.PlayOneShot(jumpClip);
            playerRb.AddForce(Vector3.up * jumpForce,ForceMode.Impulse);
            isGrounder = false;
            animator.SetTrigger("Jump_trig");
            dirt.Stop();
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounder = true;
            if(!_gameOver)dirt.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            audioSource.PlayOneShot(crashClip);
            _gameOver = true;
            Invoke("ResetGame", 5);
            dirt.Stop();
            

            if(isGrounder)
            {
                animator.SetInteger("DeathType_int", 1);
            }
            else
            {
                animator.SetInteger("DeathType_int", 2);
                playerRb.AddForce(Vector3.right* jumpForce,ForceMode.Impulse);
            }
            animator.SetBool("Death_b", true);
        }
        
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
