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
    [SerializeField] List<TheftObject> theftObjects = new List<TheftObject>();
    [SerializeField] PlayerController player;
    [SerializeField] PlayerMovementController movementController;
    [SerializeField] GameController gameController;
    [SerializeField] LevelManager levelManager;
    [SerializeField] InputManager inputManager;
    [SerializeField] MainMenuUIController mainMenuUIController;
    [SerializeField] HUDMenuScreen hudManager;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

  

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
       GameEvents.PostOnRegisterInjectables();
    }

    public void RegisterInjectable(IInjectable _injectedElement)
    {
        if (_injectedElement is TheftObject theftObject)
        {
            if (!theftObjects.Contains(theftObject))
            {
                theftObjects.Add(theftObject);
            }
        }

        if (_injectedElement is PlayerController _player)
        {
           player = _player;
        }

        if (_injectedElement is LevelManager _levelManager)
        {
            levelManager = _levelManager;
        }

        if (_injectedElement is HUDMenuScreen _hudManager)
        {
            hudManager = _hudManager;
        }

        if (_injectedElement is InputManager _inputManager)
        {
            inputManager = _inputManager;
        }

        if( _injectedElement is PlayerMovementController _movementController)
        {
            movementController = _movementController;
        }

        if (_injectedElement is MainMenuUIController _mainMenuUIController)
        {
            mainMenuUIController = _mainMenuUIController;
        }

        if (_injectedElement is GameController _gameController)
        {
            gameController = _gameController;
        }



        // Inject dependencies into the registered elements
        gameController?.Initialize(new IInjectable[] { mainMenuUIController, hudManager, player, levelManager });
        hudManager?.Initialize(new IInjectable[] { player, levelManager });
        player?.Initialize(new IInjectable[] { inputManager });
        movementController?.Initialize(new IInjectable[] { inputManager, player });

        foreach (TheftObject theftObj in theftObjects)
        {
            theftObj.Initialize(new IInjectable[] { levelManager });
        }




    }




}
