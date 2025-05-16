using System.Collections;
using UnityEngine;

public class interactDoorRotate : MonoBehaviour, IInteractable
{
    [SerializeField] MonoBehaviour playerMovementScript;

    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    public AudioSource doorOpen;
    public GameObject currentObject;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _closedRotation = transform.rotation;
        //calculates the open position of the door
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void IInteractable.Interact()
    {
        if(playerMovementScript.enabled == false){
            playerMovementScript.enabled = true;
        }
        BoxCollider boxCollider = currentObject.GetComponent<BoxCollider>();

        //makes sure no on going animations by stopping current coroutine
        if(_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(ToggleDoor());
        doorOpen.Play();

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
    }

    private IEnumerator ToggleDoor(){
        //determines whether door should open or closed based on current state
        Quaternion targetRotation = isOpen ? _closedRotation : _openRotation;

        isOpen = !isOpen;

        //create door opening transition
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = targetRotation;

        InteractionManager.Instance.EndInteraction();
    }
}
