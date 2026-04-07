using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Old : MonoBehaviour
{
    private CharacterController controller;
    private Vector2 moveInput;
    public float speed;

    private Vector3 playerVelocity;
    private bool grounded;
    public float gravity = -9.8f;
    public float jumpForce = 2f;

    public Camera cam;
    private Vector2 lookPos;
    private float xRotation;
    public float xSens = 30f;
    public float ySens = 30f;

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jump();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookPos = context.ReadValue<Vector2>();
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;
    }

    private void FixedUpdate()
    {
        movePlayer();
        playerLook();
    }

    public void movePlayer()
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = moveInput.x;
        moveDirection.z = moveInput.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if (grounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void jump()
    {
        if (grounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -3f * gravity);
        }
    }

    public void playerLook()
    {
        xRotation = (lookPos.y * Time.deltaTime) * ySens;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (lookPos.x * Time.deltaTime) * xSens);
    }
}
