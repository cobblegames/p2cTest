using System;
using UnityEngine;

public class GameController : MonoBehaviour, IInjectable
{
    [SerializeField] private GameState gameState = GameState.MainMenu;
    public GameState CurrentGameState => gameState;

    [SerializeField] private MainMenuUIController _mainMenuUIController;
    [SerializeField] private HUDMenuScreen _hudManager;
    [SerializeField] private PlayerController _player;
    [SerializeField] private LevelManager _levelManager;

    private void OnEnable()
    {
        GameEvents.OnChangeGameState -= ChangeGameState;
    }

    private void OnDisable()
    {
        GameEvents.OnChangeGameState -= ChangeGameState;
    }

    public void Initialize(IInjectable[] _injectedElements)
    {
        _mainMenuUIController = _injectedElements[0] as MainMenuUIController;
        _hudManager = _injectedElements[1] as HUDMenuScreen;
        _player = _injectedElements[2] as PlayerController;
        _levelManager = _injectedElements[3] as LevelManager;

    }

    public void FirstRun()
    {
        GameEvents.PostOnChangeGameState(GameState.MainMenu);
    }


    void ChangeGameState(GameState _gameState)
    {
       gameState = _gameState;

        switch(gameState)
        {
            case GameState.MainMenu:
                Handle_MainMenu();
                break;
            case GameState.InGame:
                Handle_InGame();
                break;
            case GameState.Winning:
                // Handle winning state
                break;
            case GameState.Losing:
                // Handle losing state
                break;
            case GameState.RadialMenu:
                Handle_RadialMenu();
                break;
            default:
                Debug.LogWarning($"Unhandled game state: {gameState}");
                break;
        }

    }

    private void Handle_MainMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void Handle_InGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Handle_RadialMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}