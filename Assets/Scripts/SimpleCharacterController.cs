using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public Transform cameraTransform;

    private float xRotation = 0f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    Vector2 moveInput;

    private void OnEnable()
    {
        InputManager.Instance.OnLook += HandleLook;
        InputManager.Instance.OnMove += HandleMovement;
        InputManager.Instance.OnJump += HandleJump;
    }

    private void OnDisable()
    {
        if (InputManager.Instance)
        {
            InputManager.Instance.OnLook -= HandleLook;
            InputManager.Instance.OnMove -= HandleMovement;
            InputManager.Instance.OnJump -= HandleJump;
        }
  
        
    }

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }



    void HandleLook(Vector2 _input)
    {
        float mouseX = _input.x *  Time.deltaTime;
        float mouseY = _input.y *  Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = moveInput.x;
        float z = moveInput.y;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovement(Vector2 _input)
    {
        moveInput = _input;

    }

    void HandleJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}