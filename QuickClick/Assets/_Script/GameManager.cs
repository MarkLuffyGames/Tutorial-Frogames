using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> targets;

    int score = 0;
    public TextMeshProUGUI scoreText;
    int healt = 3;
    public TextMeshProUGUI healtText;

    public Canvas gameOverCanvas;
    public bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnManager", 2, 1);
        UpdateScore(score);
        gameOverCanvas.gameObject.SetActive(false);
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
}
