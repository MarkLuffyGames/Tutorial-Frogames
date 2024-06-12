using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public GameObject player;
    public CanvasGroup endingLevelCanvas;

    private bool isGameOver;
    private float timer;
    private float displayImagenDuration = 1f;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            isGameOver = true;
        }
    }

    private void Update()
    {
        if(isGameOver)
        {
            timer += Time.deltaTime;
            endingLevelCanvas.alpha = timer / fadeDuration;

            if(timer > fadeDuration + displayImagenDuration)
            {
                EndingLevel();
            }
        }
    }

    void EndingLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
