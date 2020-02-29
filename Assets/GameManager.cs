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
    public int score = 0;
    public Text scoreText;
    public Text playAgainText;
    public Text hightScoreText;

    public float lengthOfDay = 10.0f;
    public float currentTimeOfDay = 0.0f;

    public GameState state;

    private void Start()
    {
        hightScoreText.text = getHighScore().ToString();
    }

    void Update()
    {
        currentTimeOfDay = Time.timeSinceLevelLoad;

        if (state == GameState.Gameover && Input.GetKey(KeyCode.Return))
        {
            if (score > getHighScore())
                setHighScore(score);
            
            SceneManager.LoadScene("Main");
        }

        if(currentTimeOfDay < lengthOfDay) 
        {
            scoreText.text = (score).ToString("0");
        }
        else
        {
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