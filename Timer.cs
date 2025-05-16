using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    private Color originalColor;

    private void Start()
    {
        if (timerText != null){
            originalColor = timerText.color;
        }
    }

    public void SubtractTime(float timeSubtracted){
        remainingTime = Mathf.Max(remainingTime - timeSubtracted, 0);
        StartCoroutine(FlashRed());
    }

    //function to flash timer text red when time is subtracted
    private System.Collections.IEnumerator FlashRed(){
        if (timerText == null) yield break;

        timerText.color = Color.red;
        yield return new WaitForSeconds(0.8f);
        timerText.color = originalColor;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime-=Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            remainingTime = 0;            
            timerText.color = Color.red;

            //navigates to game over procedure
            StartCoroutine(HandleGameOver());
        }
        GameManager.Instance.UpdateRemainingTime(remainingTime);
        
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private System.Collections.IEnumerator HandleGameOver(){
        GameManager.Instance.SaveScore();

        yield return new WaitForSeconds(0.1f);
        //navigates to game over screen
        SceneManager.LoadScene("DeathScene");
    }
}
