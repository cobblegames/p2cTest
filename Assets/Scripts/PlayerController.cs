using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInjectable
{
    [Header("Injected dependencies")]
    [SerializeField] private InputManager _inputManager;

    [Header("Local dependencies")]
    [SerializeField] private InteractionController interactionController;

    [SerializeField] private PlayerMovementController _movement;

    [SerializeField] private PlayerAlarmStatus playerStatus;
    public PlayerAlarmStatus PlayerStatus => playerStatus;
    [SerializeField] private PlayerAction currentPlayerAction;
    public PlayerAction PlayerAction => currentPlayerAction;

    [SerializeField] private Transform carringReferencePoint;
    public Transform CarryingPoint => carringReferencePoint;

    private TheftObject currentTheftObject;
    public TheftObject CurrentTheftObject => currentTheftObject;

    private void OnEnable()
    {
       
    }

    private void OnDisable()
    {
      
        UnregisterEvents();
    }

  

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
    }

    private void UnregisterEvents()
    {
        _inputManager.OnUseAction -= ExecuteAction;
        GameEvents.OnPlayerDetected -= Handle_PlayerDetectedState;
        GameEvents.OnChangePlayerAction -= ChangePlayerAction;
    }

    private void Handle_PlayerDetectedState(bool isDetected)
    {
        if (isDetected)
        {
            playerStatus = PlayerAlarmStatus.Detected;
            if (CurrentTheftObject != null)
            {
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
    }

    public void RegisterTheftObject(TheftObject theftObject)
    {
        if (currentTheftObject != null)
        {
            Debug.LogWarning("Already carrying an object. Cannot register a new one.");
            return;
        }
        currentTheftObject = theftObject;
    }
}