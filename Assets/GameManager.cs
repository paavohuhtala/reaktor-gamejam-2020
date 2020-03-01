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

    public float lengthOfDay = 60.0f;
    public float currentTimeOfDay = 0.0f;
    public float MusicStartsSpeedingUpAfter = 30.0f;

    public AudioSource worldRecordAudio;
    private bool worldRecordSaid;

    public GameState state;

    private string highScoreVariable = "High Score";

    public AudioClip MusicStart;
    public AudioClip MusicLoop;
    public AudioClip MusicEnd;

    public AudioSource MusicPlayer;

    public float MusicSpeedupFactor = 0.5f;

    public Gradient SkyGradient;

    private void Start()
    {
        if (state == GameState.Running)
        {
            MusicPlayer.clip = MusicStart;
            MusicPlayer.Play();
            hightScoreText.text = getHighScore().ToString();
        }
    }

    void Update()
    {
        if (state == GameState.Starting)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                state = GameState.Running;
                SceneManager.LoadScene("Main");
            }

            return;
        }

        if (state == GameState.Gameover && Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("Main");
        }
        else if (state == GameState.Running)
        {
            currentTimeOfDay = Time.timeSinceLevelLoad;
            UpdateMusic();
            UpdateSky();

            if (state == GameState.Gameover && Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene("Main");
            }

            if (currentTimeOfDay < lengthOfDay)
            {
                scoreText.text = (score).ToString("0");
            }
        }

        if (currentTimeOfDay >= lengthOfDay && state == GameState.Running)
        {
            state = GameState.Gameover;
            playAgainText.text = "Press Enter to play again!";
            MusicPlayer.Stop();
            MusicPlayer.clip = MusicEnd;
            MusicPlayer.loop = false;
            MusicPlayer.pitch = 1;
            MusicPlayer.Play();
        }
    }

    public void AddScore(int newScore)
    {
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

    public void UpdateMusic()
    {
        // Start music if not playing
        if (state == GameState.Running && !MusicPlayer.isPlaying)
        {
            MusicPlayer.clip = MusicLoop;
            MusicPlayer.loop = true;
            MusicPlayer.Play();
        }

        var timeRemaining = lengthOfDay - currentTimeOfDay;

        if (timeRemaining <= MusicStartsSpeedingUpAfter)
        {
            MusicPlayer.pitch = 1.0f + (1 - (timeRemaining / MusicStartsSpeedingUpAfter)) * MusicSpeedupFactor;
        }
    }

    public void UpdateSky()
    {
        var t = currentTimeOfDay / lengthOfDay;
        var skyColor = SkyGradient.Evaluate(t);
        Camera.main.backgroundColor = skyColor;

        /*var third = lengthOfDay / 3;

        Color previousColor;
        Color nextColor;
        float t;

        if (currentTimeOfDay < third)
        {
            previousColor = MorningColor;
            nextColor = DayColor;
            t = currentTimeOfDay / third;
        }
        else if (currentTimeOfDay < 2 * third)
        {
            previousColor = DayColor;
            nextColor = DayColor;
            t = 0;
        }
        else
        {
            previousColor = DayColor;
            nextColor = EveningColor;
            t = (currentTimeOfDay - 2 * third) / third;
        }

        var skyColor = Color.Lerp(previousColor, nextColor, t);*/
        // Camera.main.backgroundColor = skyColor;
    }
}