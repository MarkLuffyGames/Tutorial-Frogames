using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum GameStates { mainMenu, inGame, gameOver }

public class GameManager : MonoBehaviour
{
    public int coin = 0;
    private int totalCoin;
    private int score;

    public GameStates gameState = GameStates.mainMenu;
    public static GameManager instance;

    public GameObject menuCanvas;
    public GameObject inGameCanvas;
    public GameObject gameOverCanvas;

    public Text scoreTextInGame;
    public Text maxScoreTextInGame;
    public Text coinTextInGame;

    public Text scoreTextGameOver;
    public Text maxScoreTextGameOver;
    public Text coinTextGameOver;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameState = GameStates.mainMenu;
    }

    public void MainMenu()
    {
        ChangeGameEstate(GameStates.mainMenu);
    }
    public void StartGame()
    {
        LevelGenerator.Instance.RemoveAllBlock();
        LevelGenerator.Instance.Awake();
        PlayerController.instance.Start();
        ChangeGameEstate(GameStates.inGame);
        score = 0;
        scoreTextInGame.text = $"Score {score}";
        coin = 0;
        coinTextInGame.text = $"{coin}";
        maxScoreTextInGame.text = $"Max Score {PlayerPrefs.GetFloat("MaxScore", 0)}";

        InvokeRepeating("AddScore", 0.2f, 0.2f);
    }

    public void GameOver()
    {
        CancelInvoke("AddScore");
        ChangeGameEstate(GameStates.gameOver);
        scoreTextGameOver.text = $"Score {score}";
        maxScoreTextGameOver.text = $"Max Score {PlayerPrefs.GetFloat("MaxScore", 0)}";
        coinTextGameOver.text = $"{coin}";
    }

    void ChangeGameEstate(GameStates state)
    {
        if(state == GameStates.mainMenu)
        {
            menuCanvas.SetActive(true);
            inGameCanvas.SetActive(false);
            gameOverCanvas.SetActive(false);
        }
        else if(state == GameStates.inGame)
        {
            menuCanvas.SetActive(false);
            inGameCanvas.SetActive(true);
            gameOverCanvas.SetActive(false);
        }
        else if(state == GameStates.gameOver)
        {
            menuCanvas.SetActive(false);
            inGameCanvas.SetActive(false);
            gameOverCanvas.SetActive(true);
        }
        
        gameState = state;
    }

    void AddScore()
    {
        score++;
        scoreTextInGame.text = $"Score {score}";
        if (score > PlayerPrefs.GetFloat("MaxScore", 0))
        {
            PlayerPrefs.SetFloat("MaxScore", score);
            maxScoreTextInGame.text = $"Max Score {PlayerPrefs.GetFloat("MaxScore", 0)}";
        }
        
    }
    public void AddCoin()
    {
        coin++;
        coinTextInGame.text = $"{coin}";
    }
}
