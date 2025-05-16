using UnityEngine;
using UnityEngine.UI;

public class interactTest : MonoBehaviour, IInteractable
{
    public GameObject puzzleCanvas;
    public GameObject playerHUD;
    [SerializeField] MonoBehaviour playerGunScript;
    void IInteractable.Interact()
    {
        if (puzzleCanvas != null)
        {
            //shows bomb puzzle UI and disables player HUD
            puzzleCanvas.SetActive(true);
            playerHUD.SetActive(false);

            //unlocks mouse cursor when puzzle is active
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerGunScript.enabled = false;
        }
        else
        {
            Debug.LogWarning("Puzzle canvas not assigned");
        }
    }
    
    public void CloseBombPuzzle(){
        puzzleCanvas.SetActive(false); 
        playerHUD.SetActive(true);

        playerGunScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InteractionManager.Instance.EndInteraction();
    }
}
