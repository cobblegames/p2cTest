using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour, IInjectable
{
    [Header ("Player Settings")]
    [SerializeField] private PlayerData playerData;

    [Header("Local References - Drag in Inspector")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Transform cameraTransform;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private bool lockMovement = true;
    private Vector2 moveInput;
    private float speedMultiplier = 1f; // Speed multiplier for movement
    private bool gameIsStarted = false;

    InputManager _inputManager;


    private void OnEnable()
    {
     GameEvents.OnRegisterInjectables += RegisterInjectable;

    }

    private void OnDisable()
    {
        GameEvents.OnRegisterInjectables -= RegisterInjectable;
        UnregisterEvents();
    }


    public void RegisterInjectable()
    {
      InterfaceDependencyInjector.Instance.RegisterInjectable(this);
    }


    public void Initialize(IInjectable[] _injectedElements)
    {
        _inputManager = _injectedElements[0] as InputManager;

        RegisterEvents();
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    private void RegisterEvents()
    {
        _inputManager.OnLook += HandleLook;
        _inputManager.OnMove += HandleMovement;
        _inputManager.OnJump += HandleJump;
        _inputManager.OnShowRadialMenu += () => lockMovement = true;
        _inputManager.OnHideRadialMenu += () => lockMovement = false;

        GameEvents.OnGameStart += HandleStartGame;
        GameEvents.OnChangePlayerAction += Handle_ChangePlayerAction;

    }

 

    private void UnregisterEvents()
    {
        _inputManager.OnLook -= HandleLook;
        _inputManager.OnMove -= HandleMovement;
        _inputManager.OnJump -= HandleJump;
        _inputManager.OnShowRadialMenu -= () => lockMovement = true;
        _inputManager.OnHideRadialMenu -= () => lockMovement = false;

        GameEvents.OnGameStart -= HandleStartGame;
        GameEvents.OnChangePlayerAction -= Handle_ChangePlayerAction;
    }


    private void Handle_ChangePlayerAction(PlayerAction action)
    {
        
    }

    private void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    private void HandleStartGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsStarted = true;
        lockMovement = false; // Unlock movement when the game starts
        StartCoroutine(MovementCorutine());
    }

    private void HandleLook(Vector2 _input)
    {
        if (lockMovement)
            return;

        float mouseX = _input.x * Time.deltaTime;
        float mouseY = _input.y * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private IEnumerator MovementCorutine()
    {
        while (gameIsStarted)
        {
            isGrounded = controller.isGrounded;
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;
            float x = moveInput.x;
            float z = moveInput.y;
            if (lockMovement)
            {
                x = 0f;
                z = 0f;
            }
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * playerData.MoveSpeed * speedMultiplier * Time.deltaTime);
            velocity.y += playerData.Gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            yield return new WaitForEndOfFrame(); // Wait for the next frame
        }
    }

    private void HandleMovement(Vector2 _input)
    {
        moveInput = _input;
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(playerData.JumpHeight * -2f * playerData.Gravity);
        }
    }

}