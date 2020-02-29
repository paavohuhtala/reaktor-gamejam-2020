using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Running,
    Gameover
}

public class GameManager : MonoBehaviour
{
    public float timeLeft = 5.0f;
    public int score = 0;
    public Text startText; // used for showing countdown from 3, 2, 1 
    public Text scoreText;
    public Text playAgainText;
    public Text hightScoreText;

    public GameState state;

    private void Start()
    {
        hightScoreText.text = getHighScore().ToString();
    }

    void Update()
    {
        if(state == GameState.Gameover && Input.GetKey(KeyCode.Return))
        {
            if (score > getHighScore())
                setHighScore(score);
            
            SceneManager.LoadScene("Main");
        }

        if(timeLeft > 0) 
        {
            timeLeft -= Time.deltaTime;
            startText.text = (timeLeft).ToString("0");
            scoreText.text = (score).ToString("0");
        }
        else
        {
            startText.text = "Game Over!";
            playAgainText.text = "Press Enter to play again!";
            state = GameState.Gameover;
        }
    }

    public void AddScore(int newScore) {
        score += newScore;

        if (score > getHighScore())
            setHighScore(score);
    }

    private int getHighScore()
    {
        return PlayerPrefs.GetInt("High Score");
    }

    private void setHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt("High Score", newHighScore);
        hightScoreText.text = getHighScore().ToString();
    }
}