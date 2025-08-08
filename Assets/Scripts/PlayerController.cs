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

        interactionController.TryInteract(currentPlayerAction);
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
