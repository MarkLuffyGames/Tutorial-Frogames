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
    public CanvasGroup caughtCanvas;
    public AudioSource exitAudio, caugthAudio;

    private bool hasAudioPlayed;
    private bool isPlayerExit, isPlayerCaught;
    private float timer;
    private float displayImagenDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            isPlayerExit = true;
        }
    }

    private void Update()
    {
        if(isPlayerExit)
        {
            EndLevel(endingLevelCanvas, exitAudio);
        }
        else if(isPlayerCaught)
        {
            EndLevel(caughtCanvas, caugthAudio);
        }
    }

    void EndLevel(CanvasGroup imageCanvas, AudioSource audioSource)
    {
        timer += Time.deltaTime;
        imageCanvas.alpha = timer / fadeDuration;

        if (!hasAudioPlayed)
        {
            hasAudioPlayed = true;
            audioSource.Play();
        }

        if (timer > fadeDuration + displayImagenDuration)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
    }

    public void CatchPlayer()
    {
        isPlayerCaught = true;
    }
}
