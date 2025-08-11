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

    }
   
}