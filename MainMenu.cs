using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame(){
        if(GameManager.Instance != null){
            GameManager.Instance.GenerateNewBombCode();
        }
        PauseMenu.isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quitGame()
    {
        Debug.Log("Application has quit");
        Application.Quit();
    }
}
