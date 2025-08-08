using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Utils;
public class InputManager : MonoBehaviourSingleton<InputManager>
{
    public event System.Action<Vector2> OnMove;
    public event System.Action<Vector2> OnLook;
    public event System.Action OnJump;
    public event System.Action OnUseAction;
    public event System.Action OnShowRadialMenu;
    public event System.Action OnHideRadialMenu;
    public event System.Action OnMenu;
    

    [Header("Input Configuration")]
    public PlayerInput playerInput;
    public InputActionAsset defaultInputActions;


    [Header("Control Settings")]
    public bool invertYAxis = false;
    public bool invertXAxis = false;
    [Range(0.1f, 100f)] public float xlookSensitivity = 50f;
    [Range(0.1f, 100f)] public float ylookSensitivity = 50f;
    // Action names
    private const string MOVE_ACTION = "Move";
    private const string LOOK_ACTION = "Look";
    private const string JUMP_ACTION = "Jump"; 
    private const string USE_ACTION = "UseAction";
    private const string RADIAL_MENU_ACTION = "RadialMenuAction";    
    private const string MENU_ACTION = "GameMenuAction";
    

    protected override void Awake()
    {
        base.Awake();
        transform.parent = null;
        InitializeInputSystem();
    }

    private void InitializeInputSystem()
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
        var moveAction = playerInput.actions[MOVE_ACTION];
        var lookAction = playerInput.actions[LOOK_ACTION];
        var jumpAction = playerInput.actions[JUMP_ACTION];
        var useAction = playerInput.actions[USE_ACTION];
        var radialMenuAction = playerInput.actions[RADIAL_MENU_ACTION];
        var menuAction = playerInput.actions[MENU_ACTION];
        

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

        lookAction.canceled += ctx =>
        {
            OnLook?.Invoke(Vector2.zero);

        };

        // Discrete inputs
        jumpAction.performed += _ => OnJump?.Invoke();
        useAction.performed += _ => OnUseAction?.Invoke();
        radialMenuAction.performed += _ => OnShowRadialMenu?.Invoke();
        radialMenuAction.canceled += _ => OnHideRadialMenu?.Invoke();
        menuAction.performed += _ => OnMenu?.Invoke();
        
    }


    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupInputCallbacks();
    }


}
