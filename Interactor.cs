using UnityEngine;

interface IInteractable {
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    //interact ray is cast from the interactorSource
    public Transform interactorSource;
    public float interactRange;
    public GameObject interactUI;
    private IInteractable currentInteractable;
    // private bool isInteracting = false;

    // Update is called once per frame
    void Update()
    {
        // if (isInteracting){
        //     interactUI.SetActive(false);
        //     return; //don't allow new interactions while interacting
        // }
        
        if (!InteractionManager.Instance.CanInteract()){
            //if currently interacting (puzzle active), stop new interactions
            InteractionManager.Instance.ShowInteractText(false);
            return;
        }

        bool foundInteractable = false;

        //creates raycast
        Ray r = new Ray(interactorSource.position, interactorSource.forward);
        if (Physics.Raycast(r, out RaycastHit hitInfo, interactRange)){
            //if the raycast detects a collider
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj)){
                foundInteractable = true;
                currentInteractable = interactObj;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    InteractionManager.Instance.StartInteraction();
                    interactObj.Interact();
                }
            }
        }
        //shows or hides press E based on whether looking at something interactable or not
        
        // interactUI.SetActive(foundInteractable);
        InteractionManager.Instance.ShowInteractText(foundInteractable);

        if (Input.GetKeyDown(KeyCode.P)) // press 'P' to manually reset
        {
            InteractionManager.Instance.EndInteraction();
        }
    }

    // public void EndInteraction(){
    //     isInteracting = false;
    // }
    
}
