using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get;
        private set;
    } = null;
    public GameObject gameOverUI;
    public Text scoreText;
    public Text resultText;
    public bool isGameOver = false;
    public int score = 0;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (isGameOver)
        {
            GameOver();
        }
        EndGame();
    }

    public void GameOver()
    {
        resultText.text = "Result : " + this.score;
        gameOverUI.SetActive(true);
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = "score : " + this.score;
    }
    private void EndGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
