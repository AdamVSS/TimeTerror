using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public InputAction lookAction;

    private float xRotation = 0f; //stores vertical rotation
    private Vector2 lookInput; //stores mouse input
    PlayerInput playerInput;
    InputAction lookingAction;
    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        lookingAction = playerInput.actions.FindAction("Look");

        Cursor.lockState = CursorLockMode.Locked; //locks cursor to center of the screen
    }

    private void Update()
    {
        OnLook();
    }

    public void OnLook()
    {
        lookInput = lookingAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //prevents camera flipping (by clamping)

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //rotate camera up/down
        playerBody.Rotate(Vector3.up * mouseX); //rotate player left/right
    }
}
