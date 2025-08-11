/* Summary:

    InterfaceDependencyInjector is a singleton class that manages the registration and unregistration of injectable elements in the game.
    Phase 1: Registers injectable elements like TheftObject, PlayerController, LevelManager, HUDManager, and InputManager.
    Phase 2: Injects these dependencies into the respective classes when they are registered.
    Phase 3: Sends event that all injectables are registered, allowing other systems to access these dependencies and start the game logic.
 
*/

using System.Collections.Generic;
using Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class InterfaceDependencyInjector : MonoBehaviourSingleton<InterfaceDependencyInjector>
{
    [Header("This needs to be dragged in the editor")]


    [SerializeField] PlayerController player;
    [SerializeField] PlayerMovementController movementController;
    [SerializeField] GameController gameController;
    [SerializeField] LevelManager levelManager;
    [SerializeField] InputManager inputManager;
    [SerializeField] MainMenuUIController mainMenuUIController;
    [SerializeField] HUDMenuScreen hudManager;
    [SerializeField] RadialMenu radialMenu;
   

    private void Start()
    {
        InitializeInjectables();
    }




    void InitializeInjectables()
    {
        gameController?.Initialize(new IInjectable[] { mainMenuUIController, hudManager, player, levelManager });
        hudManager?.Initialize(new IInjectable[] { player, levelManager });
        player?.Initialize(new IInjectable[] { inputManager });
        movementController?.Initialize(new IInjectable[] { inputManager, player });
        levelManager?.Initialize(new IInjectable[] { hudManager});
        inputManager?.Initialize(new IInjectable[0]);
        mainMenuUIController.Initialize(new IInjectable[0]);
        radialMenu?.Initialize(new IInjectable[] { inputManager});

        Debug.Log("All injectables initialized successfully.");
        
        gameController.FirstRun();
        
    }
}