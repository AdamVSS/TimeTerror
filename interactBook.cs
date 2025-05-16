using UnityEngine;

public class interactBook : MonoBehaviour, IInteractable
{
    public GameObject bookCanvas;
    public GameObject playerHUD;
    [SerializeField] MonoBehaviour playerGunScript;
    [SerializeField] MonoBehaviour playerMovementScript;
    void IInteractable.Interact()
    {
        if (bookCanvas != null){
            //shows bomb puzzle UI and disables player HUD
            bookCanvas.SetActive(true); 
            playerHUD.SetActive(false);
            
            playerGunScript.enabled = false;
            playerMovementScript.enabled = false;
            
            Time.timeScale = 0f;

            //unlocks mouse cursor when puzzle is active
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseBook(){
        bookCanvas.SetActive(false); 
        playerHUD.SetActive(true);

        playerGunScript.enabled = true;
        playerMovementScript.enabled = true;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InteractionManager.Instance.EndInteraction();
    }
}

