using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerMovementController movement;

    PlayerAlarmStatus playerStatus;
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
    }

    private void OnDisable()
    {
        if(InputManager.Instance)
        {
            InputManager.Instance.OnUseAction -= ExecuteAction;
        }
      
    }
    private void Start()
    {
        InitRadialMenu();
    }


  
    private void ChangePlayerAction(PlayerAction newAction)
    {
        currentPlayerAction = newAction;
    }

    private void ExecuteAction()
    {
        Debug.Log($"Executing action: {currentPlayerAction}");

        if(currentPlayerAction== PlayerAction.Drop)
        {
            if (currentTheftObject != null)
            {
                Debug.Log($"Dropping object: {currentTheftObject.gameObject.name}");
                currentTheftObject.Drop();
                currentTheftObject = null;
            }
            return;
        }else
        {
            interactionController.TryInteract(this);
        }
        
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
