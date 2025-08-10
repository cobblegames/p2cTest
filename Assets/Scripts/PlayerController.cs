using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovementController movement;

    [SerializeField] PlayerAlarmStatus playerStatus;
    public PlayerAlarmStatus PlayerStatus => playerStatus;
    [SerializeField] PlayerAction currentPlayerAction;
    public PlayerAction PlayerAction => currentPlayerAction;
    public RadialMenu radialMenu;

    [SerializeField] List<MenuActionData> menuActions;
    [SerializeField] InteractionController interactionController;


    [SerializeField] Transform carringReferencePoint;
    public Transform CarryingPoint => carringReferencePoint;

    TheftObject currentTheftObject;
    public TheftObject CurrentTheftObject  => currentTheftObject;


    private void OnEnable()
    {
        InputManager.Instance.OnUseAction += ExecuteAction;
        GameEvents.OnPlayerDetected += Handle_PlayerDetectedState;
    }

 

    private void OnDisable()
    {
        GameEvents.OnPlayerDetected -= Handle_PlayerDetectedState;
        if (InputManager.Instance)
        {
            InputManager.Instance.OnUseAction -= ExecuteAction;
        }
      
    }
    private void Start()
    {
        InitRadialMenu();
        GameManager.Instance.RegisterPlayer(this);
    }

    private void Handle_PlayerDetectedState(bool isDetected)
    {
       if(isDetected)
        {
           playerStatus = PlayerAlarmStatus.Detected;
           if(CurrentTheftObject!=null)
            {
                CurrentTheftObject.Drop();
                currentTheftObject = null;
                movement.SetSpeedMultiplier(1f); // Reset speed when not carrying an object
            }
              

        }
        else
        {
            playerStatus = PlayerAlarmStatus.NotDetected;
           
        }
    }

    private void ChangePlayerAction(PlayerAction newAction)
    {
        currentPlayerAction = newAction;
    }

    private void ExecuteAction()
    {
        Debug.Log($"Executing action: {currentPlayerAction}");
        interactionController.TryInteract(this);
         
    }
    public void UnregisterTheftObject()
    {
        currentTheftObject = null;
        movement.SetSpeedMultiplier(1f); // Reset speed when not carrying an object
    }

   public void RegisterTheftObject(TheftObject theftObject)
    {
        if (currentTheftObject != null)
        {
            Debug.LogWarning("Already carrying an object. Cannot register a new one.");
            return;
        }
        currentTheftObject = theftObject;
        movement.SetSpeedMultiplier(0.5f); // Slow down when carrying an object
    }

    private void InitRadialMenu()
    {
        var actions = new List<RadialMenu.MenuAction>();
        foreach (var action in menuActions)
        {
            actions.Add(new RadialMenu.MenuAction
            {
                name = action.Action.ToString(),
                action = () =>
                {
                    ChangePlayerAction(action.Action);
                    action.ExecuteAction();
                }
            });
        }

        radialMenu.CreateMenu(actions);
    }

}
