using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BombDefusal : MonoBehaviour
{

    public TMP_Text enteredNumbers;
    public string correctCode;
    private string currentInput = "";
    public AudioSource buzz;
    public Timer timer;
    public float lockoutTime = 5f;
    private float nextAllowedAttempt = 0f;

    void Start()
    {
        GenerateRandomCode();
    }

    public void GenerateRandomCode(){
        //generates random 4 digit code
        correctCode = GameManager.bombCode;
    }

    public void onNumberButtonPressed(string number){
        //only the amount of numbers in the correct code can be entered
        if (currentInput.Length < correctCode.Length){
            currentInput += number;
            enteredNumbers.text = currentInput;
        }
    }

    public void onDeleteButtonPressed(){
        currentInput = "";
        enteredNumbers.text = "";
    }

    public void onSubmitButtonPressed(){
        if (Time.time < nextAllowedAttempt) return;
        nextAllowedAttempt = Time.time + lockoutTime;

        if (currentInput == correctCode)
        {
            enteredNumbers.text = "Correct";
            Debug.Log("Bomb Diffused!");

            //save score and navigate to game over
            GameManager.Instance.SaveScore();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            enteredNumbers.text = "Incorrect";
            buzz.Play();

            if (timer != null)
            {
                timer.SubtractTime(10f);
                Debug.Log("Incorrect code -10s");
            }
        }
    }
}
