using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour, IInjectable
{
    [Header("Player Settings")]
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

    private InputManager _inputManager;

    public void Initialize(IInjectable[] _injectedElements)
    {
        _inputManager = _injectedElements[0] as InputManager;

        
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _inputManager.OnLook += Handle_LookEvent;
        _inputManager.OnMove += HandleMovement;
        _inputManager.OnJump += HandleJump;
        _inputManager.OnShowRadialMenu += () => lockMovement = true;
        _inputManager.OnHideRadialMenu += () => lockMovement = false;

        GameEvents.OnChangeGameState += Handle_ChangeGameState;     
        GameEvents.OnRestartGame += UnregisterEvents;
    }

    private void UnregisterEvents()
    {
        _inputManager.OnLook -= Handle_LookEvent;
        _inputManager.OnMove -= HandleMovement;
        _inputManager.OnJump -= HandleJump;
        _inputManager.OnShowRadialMenu -= () => lockMovement = true;
        _inputManager.OnHideRadialMenu -= () => lockMovement = false;

        GameEvents.OnChangeGameState -= Handle_ChangeGameState;
        GameEvents.OnRestartGame -= UnregisterEvents;
    }

    private void Handle_ChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                Handle_MainMenu();
                break;

            case GameState.InGame:
                Handle_StartGame();
                break;

            case GameState.Winning:
                Handle_WinGame();
                break;

            case GameState.Losing:
                Handle_LostGame();

                break;

            default:
                Debug.LogWarning($"Unhandled game state: {state}");
                break;
        }
    }



    private void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    private void Handle_MainMenu()
    {
        lockMovement = true; // Lock movement in the main menu
        gameIsStarted = false;
        StopAllCoroutines(); // Stop any ongoing movement coroutine
        // Maybe later reset player position or camera rotation?
    }

    private void Handle_StartGame()
    {
        lockMovement = false; // Lock movement when winning or losing
        if (!gameIsStarted)
        {
            gameIsStarted = true;
            StartCoroutine(MovementCorutine());
        }
    }

    private void Handle_LostGame()
    {
        lockMovement = true; // Lock movement when winning
        gameIsStarted = false;
        StopAllCoroutines(); // Stop any ongoing movement coroutine
    }

    private void Handle_WinGame()
    {
        lockMovement = true; // Lock movement when winning
        gameIsStarted = false;
        StopAllCoroutines(); // Stop any ongoing movement coroutine
    }

    private void Handle_LookEvent(Vector2 _input)
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