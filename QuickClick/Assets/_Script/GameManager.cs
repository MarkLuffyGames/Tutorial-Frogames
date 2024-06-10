using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> targets;

    int score = 0;
    public TextMeshProUGUI scoreText;
    int healt = 3;
    public TextMeshProUGUI healtText;

    public Canvas gameOverCanvas;
    public Canvas menu;
    public Canvas ui;
    public bool gameOver = false;

    private float spawnRate = 1;

    private void Start()
    {
        ui.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
    }
    public void StartGame(int difficulty)
    {
        menu.gameObject.SetActive(false);
        ui.gameObject.SetActive(true);
        spawnRate /= difficulty;
        InvokeRepeating("SpawnManager", 1, spawnRate);
        UpdateScore(score);
        gameOver = false;
    }

    void SpawnManager()
    {
        var index = Random.Range(0, targets.Count);
        Instantiate(targets[index], RandomPosition(), targets[index].transform.rotation);
    }

    Vector3 RandomPosition()
    {
        return new Vector3(Random.Range(-4, 4), 0);
    }

    public void UpdateScore(int scoreToAdd)
    {
        
        score += scoreToAdd;
        scoreText.text = score.ToString();
    }

    public void UpdateHealt()
    {
        if (healt > 0)
        {
            healt--;
            healtText.text = healt.ToString();
            if (healt == 0)
            {
                GameOver();
            }
        }
        
    }

    public void GameOver()
    {
        gameOver = true;
        CancelInvoke("SpawnManager");
        gameOverCanvas.gameObject.SetActive(true);
        healt = 0;
        healtText.text = healt.ToString();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
