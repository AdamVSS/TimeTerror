using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string bombCode;
    public static GameManager Instance;
    public float remainingTime;
    private static bool hasGeneratedBombCode = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if(!hasGeneratedBombCode){
                GenerateBombCode();
                hasGeneratedBombCode = true;
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void GenerateBombCode()
    {
        //Generates random 4 digit code
        bombCode = Random.Range(0, 10000).ToString("D4");
        Debug.Log("Generated Bomb Code: " + bombCode);
    }

    public void GenerateNewBombCode(){
        bombCode = Random.Range(0, 10000).ToString("D4");
        Debug.Log("New Bomb Code: " + bombCode);
    }

    public void UpdateRemainingTime(float time){
        remainingTime = time;
    }

    public void SaveScore(){
        int finalScore = Mathf.FloorToInt(remainingTime * 60);
        PlayerPrefs.SetInt("CurrentScore", finalScore);

        //update high score (if needed)
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > highScore){
            PlayerPrefs.SetInt("HighScore", finalScore);
        }

        PlayerPrefs.Save();

        Debug.Log($"Score saved: {finalScore} (High Score: {PlayerPrefs.GetInt("HighScore")})");
    }
}
