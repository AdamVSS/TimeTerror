using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    private Vector3 velocity;
    private bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f; //distance to check ground
    public LayerMask groundMask; //what layer is considered the ground

    private Vector2 moveInput;
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");

        jumpAction.Enable();
    }

    //Update is called once per frame
    void Update()
    {
        MovePlayer();

        //Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f; //keeps player sticking to ground
        }

        if(jumpAction.triggered && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //apply gravity only when not grounded
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    void MovePlayer(){
        moveInput = moveAction.ReadValue<Vector2>();
        
        //move the player
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * speed * Time.deltaTime);
    }
}
