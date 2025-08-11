using UnityEngine;

public class GameController : MonoBehaviour, IInjectable
{
    [SerializeField] private GameState gameState = GameState.MainMenu;
    public GameState CurrentGameState => gameState;

    [Header ("Injected Dependencies")]
    [SerializeField] private MainMenuUIController _mainMenuUIController;
    [SerializeField] private HUDMenuScreen _hudManager;
    [SerializeField] private PlayerController _player;
    [SerializeField] private LevelManager _levelManager;


    public void Initialize(IInjectable[] _injectedElements)
    {
        _mainMenuUIController = _injectedElements[0] as MainMenuUIController;
        _hudManager = _injectedElements[1] as HUDMenuScreen;
        _player = _injectedElements[2] as PlayerController;
        _levelManager = _injectedElements[3] as LevelManager;

        RegisterEvents(); 
    }

    void RegisterEvents()
    {
        GameEvents.OnRestartGame += UnregisterEvents;
        GameEvents.OnChangeGameState += ChangeGameState;
    }

    void UnregisterEvents()
    {
        GameEvents.OnRestartGame -= UnregisterEvents;
        GameEvents.OnChangeGameState -= ChangeGameState;
    }

    public void FirstRun()
    {
        GameEvents.PostOnChangeGameState(GameState.MainMenu);
    }

    private void ChangeGameState(GameState _gameState)
    {
        gameState = _gameState;

        switch (gameState)
        {
            case GameState.MainMenu:
                Handle_MainMenu();
                break;

            case GameState.InGame:
                Handle_InGame();
                break;

            case GameState.Winning:
                Handle_WinGame();
                break;

            case GameState.Losing:
                Handle_LostGame();
                break;

            case GameState.RadialMenu:
                Handle_RadialMenu();
                break;

            default:
                Debug.LogWarning($"Unhandled game state: {gameState}");
                break;
        }
    }

    private void Handle_WinGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _player.PlayerCamera.gameObject.SetActive(false);
    }

    private void Handle_LostGame()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _player.PlayerCamera.gameObject.SetActive(false);
    }

    private void Handle_MainMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _player.PlayerCamera.gameObject.SetActive(false);
    }

    private void Handle_InGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _player.PlayerCamera.gameObject.SetActive(true);
    }

    private void Handle_RadialMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _player.PlayerCamera.gameObject.SetActive(true);
    }
}