using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour, IInjectable
{

    #region System Events - this way we can separate the input system from the game logic
    public event System.Action<Vector2> OnMove;
    public event System.Action<Vector2> OnLook;
    public event System.Action<Vector2> OnPointerMove;
    public event System.Action OnJump;
    public event System.Action OnUseAction;
    public event System.Action OnShowRadialMenu;
    public event System.Action OnHideRadialMenu;
    public event System.Action OnMenu;
    #endregion

    #region New Input System Configuration
    [Header("Input Configuration")]
    public PlayerInput playerInput;
    public InputActionAsset defaultInputActions;

    private InputAction moveAction;
    private InputAction pointerMoveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction useAction;
    private InputAction radialMenuAction;
    private InputAction menuAction;

    #endregion

    [Header("Control Settings")]
    [SerializeField] private bool invertYAxis = false;
    [SerializeField] private bool invertXAxis = false;
    [SerializeField] [Range(0.1f, 100f)] private float xlookSensitivity = 50f;
    [SerializeField] [Range(0.1f, 100f)] private float ylookSensitivity = 50f;

    // Action names
    private const string MOVE_ACTION = "Move";

    private const string LOOK_ACTION = "Look";
    private const string POINTER_ACTION = "PointerMove";
    private const string JUMP_ACTION = "Jump";
    private const string USE_ACTION = "UseAction";
    private const string RADIAL_MENU_ACTION = "RadialMenuAction";
    private const string MENU_ACTION = "GameMenuAction";

    private void OnEnable()
    {
  
    }

    private void OnDisable()
    {
    
    }

    public void Initialize(IInjectable[] _injectedElements)
    {
        playerInput = GetComponent<PlayerInput>() ?? gameObject.AddComponent<PlayerInput>();

        if (playerInput.actions == null && defaultInputActions != null)
        {
            playerInput.actions = defaultInputActions;
        }

        SetupInputCallbacks();
    }

    private void SetupInputCallbacks()
    {
        if (playerInput.actions == null)
        {
            Debug.LogError("No InputActionAsset assigned to PlayerInput");
            return;
        }

        // Set up action callbacks
        moveAction = playerInput.actions[MOVE_ACTION];
        lookAction = playerInput.actions[LOOK_ACTION];
        jumpAction = playerInput.actions[JUMP_ACTION];
        useAction = playerInput.actions[USE_ACTION];
        radialMenuAction = playerInput.actions[RADIAL_MENU_ACTION];
        menuAction = playerInput.actions[MENU_ACTION];
        pointerMoveAction = playerInput.actions[POINTER_ACTION];

        // Continuous inputs
        moveAction.performed += ctx =>
        {
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
            //    Debug.Log($"Move Input: {ctx.ReadValue<Vector2>()}");
        };
        moveAction.canceled += ctx => OnMove?.Invoke(Vector2.zero);

        lookAction.performed += ctx =>
        {
            Vector2 lookValue = ctx.ReadValue<Vector2>();
            lookValue.x = lookValue.x * xlookSensitivity;
            lookValue.y = lookValue.y * ylookSensitivity;
            if (invertXAxis) lookValue.x *= -1;
            if (invertYAxis) lookValue.y *= -1;

            OnLook?.Invoke(lookValue);
        };

        pointerMoveAction.performed += ctx =>
        {
            OnPointerMove?.Invoke(ctx.ReadValue<Vector2>());
        };

        lookAction.canceled += ctx =>
        {
            OnLook?.Invoke(Vector2.zero);
        };

        // Discrete inputs
        jumpAction.performed += _ => HandleJumpAction();
        useAction.performed += _ => HandeUseAction();
        radialMenuAction.performed += _ => OnShowRadialMenu?.Invoke();
        radialMenuAction.canceled += _ => OnHideRadialMenu?.Invoke();
        menuAction.performed += _ => HandleMenuAction();
    }

    private void HandeUseAction()
    {
        if (useAction.WasPressedThisFrame())
        {
            Debug.Log("Use action triggered");
            OnUseAction?.Invoke();
        }
    }

    private void HandleJumpAction()
    {
        if (jumpAction.WasPressedThisFrame())
        {
            OnJump?.Invoke();
        }
    }

    private void HandleMenuAction()
    {
        if (menuAction.WasPressedThisFrame())
        {
            OnMenu?.Invoke();
        }
    }
}