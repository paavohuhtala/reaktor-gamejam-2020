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
    public float timeLeft = 30.0f;
    public int score = 0;
    public Text startText; // used for showing countdown from 3, 2, 1 
    public Text scoreText;

    public GameState state;

    void Update()
    {
        if(timeLeft > 0) 
        {
            timeLeft -= Time.deltaTime;
            startText.text = (timeLeft).ToString("0");
            scoreText.text = (score).ToString("0");
        }
        else
        {
            startText.text = "Game Over!";
            state = GameState.Gameover;
        }
    }

    public void AddScore(int newScore) {
        score += newScore;
    }
}