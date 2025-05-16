using UnityEngine;

public class interactDoor : MonoBehaviour, IInteractable
{
    [SerializeField] Animator doorAnimator;
    public GameObject currentObject;
    private bool isOpen = false;
    public AudioSource doorOpen;
    [SerializeField] MonoBehaviour playerMovementScript;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        if (doorAnimator == null)
        {
            doorAnimator = GetComponent<Animator>();
        }
    }

    void IInteractable.Interact()
    {
        if (doorAnimator == null) return;
        if(playerMovementScript.enabled == false){
            playerMovementScript.enabled = true;
        }

        BoxCollider boxCollider = currentObject.GetComponent<BoxCollider>();

        //toggles the door state
        isOpen = !isOpen;
        doorAnimator.SetBool("open", isOpen); //updates animator paramater
        doorOpen.Play();
        
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }

        Invoke(nameof(EndInteractionDelayed), 0.1f);
    }

    private void EndInteractionDelayed(){
        InteractionManager.Instance.EndInteraction();
    }
}
