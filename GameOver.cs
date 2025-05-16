using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] TextMeshProUGUI highScoreText;

    void Start()
    {
        int currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentScoreText){
            currentScoreText.text = "Score: " + currentScore;
        }
        if (highScoreText){
            highScoreText.text = "High Score: " + highScore;
        }
    }

    public void retryGame()
    {
        GameManager.Instance.GenerateNewBombCode();
        SceneManager.LoadScene("MainScene");
    }

    public void mainMenuScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void quitGame()
    {
        Debug.Log("Application has quit");
        Application.Quit();
    }
}
