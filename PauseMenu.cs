using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField] MonoBehaviour gunSystemScript;
    
    public static bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if (isPaused)
            {
                ResumeGame();
            } 
            else
            {
                PauseGame();
            } 
        }
    }

    //functions activated by buttons on pause menu or by pressing escape key
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gunSystemScript.enabled = true;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gunSystemScript.enabled = false;
        isPaused = true;    
    }

    public void QuitGame(){
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void MainMenuNavigate(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
