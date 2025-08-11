/* Summary:

    InterfaceDependencyInjector is a singleton class that manages the registration and unregistration of injectable elements in the game.
    Phase 1: Registers injectable elements like TheftObject, PlayerController, LevelManager, HUDManager, and InputManager.
    Phase 2: Injects these dependencies into the respective classes when they are registered.
    Phase 3: Sends event that all injectables are registered, allowing other systems to access these dependencies and start the game logic.

*/

using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class InterfaceDependencyInjector : MonoBehaviourSingleton<InterfaceDependencyInjector>
{
    [Header("This needs to be dragged in the editor")]

    [SerializeField] private PlayerController player;

    [SerializeField] private PlayerMovementController movementController;
    [SerializeField] private GameController gameController;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private MainMenuUIController mainMenuUIController;
    [SerializeField] private HUDMenuScreen hudManager;
    [SerializeField] private RadialMenu radialMenu;
    [SerializeField] private WinMenuScreen winMenu;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void Start()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        InitializeInjectables();
    }

    private void InitializeInjectables()
    {
        gameController?.Initialize(new IInjectable[] { mainMenuUIController, hudManager, player, levelManager });
        hudManager?.Initialize(new IInjectable[] { levelManager });
        player?.Initialize(new IInjectable[] { inputManager });
        movementController?.Initialize(new IInjectable[] { inputManager, player });
        levelManager?.Initialize(new IInjectable[] { hudManager });
        inputManager?.Initialize(new IInjectable[0]);
        mainMenuUIController.Initialize(new IInjectable[0]);
        radialMenu?.Initialize(new IInjectable[] { inputManager });
        winMenu?.Initialize(new IInjectable[] { levelManager });
        Debug.Log("All injectables initialized successfully.");

        gameController.FirstRun();
    }
}