using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    public GameObject interactUI; //reference to interaction text
    private bool isInteracting = false;
    [SerializeField] MonoBehaviour playerMovementScript;
    [SerializeField] MonoBehaviour playerCameraScript;

    private void Awake(){
        //Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanInteract(){
        return !isInteracting;
    }

    public void StartInteraction(){
        isInteracting = true;
        
        if (interactUI != null)
        {
            interactUI.SetActive(false); //hides interact text when an interaction is occuring
        }
        
        //Disables player movement
        if (playerMovementScript != null){
            playerMovementScript.enabled = false;
        }

        //disable player camera
        if (playerCameraScript != null)
        {
            playerCameraScript.enabled = false;
        }
    }

    public void EndInteraction(){
        isInteracting = false;

        Debug.Log("End Interaction is called!");
        //re-enable player movement
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }
        //re-enable player camera
        if (playerCameraScript != null)
        {
            playerCameraScript.enabled = true;
        }
    }

    public void ShowInteractText(bool show){
        if(!isInteracting && interactUI != null){
            interactUI.SetActive(show);
        }
    }
}
