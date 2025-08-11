using UnityEngine;

public class PlayerController : MonoBehaviour, IInjectable
{
    [Header("Injected dependencies")]
    [SerializeField] private InputManager _inputManager;

    [Header("Local dependencies")]
    [SerializeField] private InteractionController interactionController;
    [SerializeField] private PlayerMovementController _movement;
    [SerializeField] private Transform carringReferencePoint;
    [SerializeField] AudioSource gruntingSound;
    [SerializeField] Camera _playerCamera;
    public Camera PlayerCamera => _playerCamera;
    public Transform CarryingPoint => carringReferencePoint;


    [Header("Helper properties")]
    [SerializeField] private PlayerAlarmStatus playerStatus;
    [SerializeField] private PlayerAction currentPlayerAction;
    public PlayerAction PlayerAction => currentPlayerAction;

 

    private TheftObject currentTheftObject;
    public TheftObject CurrentTheftObject => currentTheftObject;

    public void Initialize(IInjectable[] _injectedElements)
    {
        _inputManager = _injectedElements[0] as InputManager;

        RegisterEvents();
    }

    private void RegisterEvents()
    {
        _inputManager.OnUseAction += ExecuteAction;
        GameEvents.OnPlayerDetected += Handle_PlayerDetectedState;
        GameEvents.OnChangePlayerAction += ChangePlayerAction;
        GameEvents.OnRestartGame += UnregisterEvents;
    }

    private void UnregisterEvents()
    {
        _inputManager.OnUseAction -= ExecuteAction;
        GameEvents.OnPlayerDetected -= Handle_PlayerDetectedState;
        GameEvents.OnChangePlayerAction -= ChangePlayerAction;
        GameEvents.OnRestartGame -= UnregisterEvents;
    }

    private void Handle_PlayerDetectedState(bool isDetected)
    {
        if (isDetected)
        {
            playerStatus = PlayerAlarmStatus.Detected;
            if (CurrentTheftObject != null)
            {
                GameEvents.PostOnCaughtPenalty();
                if (!gruntingSound.isPlaying)
                    gruntingSound.Play();

                CurrentTheftObject.Drop();
              
            }
        }
        else
        {
            playerStatus = PlayerAlarmStatus.NotDetected;
        }
    }

    private void ChangePlayerAction(PlayerAction action)
    {
        currentPlayerAction = action;

        Debug.Log($"Player action changed to: {action}");
    }

    private void ExecuteAction()
    {
        Debug.Log($"Executing action: {currentPlayerAction}");
        interactionController.TryInteract(this);
    }

    public void UnregisterTheftObject()
    {
        currentTheftObject = null;
        _movement.SetSpeedMultiplier(1f);
    }

    public void RegisterTheftObject(TheftObject theftObject)
    {
        if (currentTheftObject != null)
        {
            Debug.LogWarning("Already carrying an object. Cannot register a new one.");
            return;
        }
        currentTheftObject = theftObject;
        _movement.SetSpeedMultiplier(0.5f);
    }
}