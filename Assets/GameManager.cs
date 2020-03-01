using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum GameState
{
    Starting,
    Running,
    Gameover
}

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playAgainText;
    public TextMeshProUGUI hightScoreText;

    public float lengthOfDay = 10.0f;
    public float currentTimeOfDay = 0.0f;


    public AudioSource worldRecordAudio;
    private bool worldRecordSaid;

    public GameState state;

    private string highScoreVariable = "High Score";

    private void Start()
    {
        if(state == GameState.Running)
            hightScoreText.text = getHighScore().ToString();
    }

    void Update()
    {
        if(state == GameState.Starting)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                state = GameState.Running;
                SceneManager.LoadScene("Main");
            }
        }
        else if (state == GameState.Running)
        {
            currentTimeOfDay = Time.timeSinceLevelLoad;

            if (state == GameState.Gameover && Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene("Main");
            }

            if (currentTimeOfDay < lengthOfDay)
            {
                scoreText.text = (score).ToString("0");
            }
            else
            {
                playAgainText.text = "Press Enter to play again!";
                state = GameState.Gameover;
            }
        }
    }

    public void AddScore(int newScore) {
        if (state != GameState.Running)
        {
            return;
        }

        score += newScore;

        if (score > getHighScore())
        {
            setHighScore(score);

            if(!worldRecordSaid)
            {           
                worldRecordAudio.Play();
                worldRecordSaid = true;
            }
            
        }      
    }

    private int getHighScore()
    {
        return PlayerPrefs.GetInt(highScoreVariable) > 0 ? PlayerPrefs.GetInt(highScoreVariable) : 0;
    }

    private void setHighScore(int newHighScore)
    {
        PlayerPrefs.SetInt(highScoreVariable, newHighScore);
        hightScoreText.text = getHighScore().ToString();
    }
}