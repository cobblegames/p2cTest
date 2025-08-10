using UnityEngine;

public class GameController : MonoBehaviour, IInjectable
{
    private GameState gameState = GameState.MainMenu;
    public GameState CurrentGameState => gameState;

    [SerializeField] private MainMenuUIController _mainMenuUIController;
    [SerializeField] private HUDMenuScreen _hudManager;
    [SerializeField] private PlayerController _player;
    [SerializeField] private LevelManager _levelManager;

    private void OnEnable()
    {
        GameEvents.OnRegisterInjectables += RegisterInjectable;
    }

    private void OnDisable()
    {
        GameEvents.OnRegisterInjectables -= RegisterInjectable;
    }

    public void Initialize(IInjectable[] _injectedElements)
    {
        _mainMenuUIController = _injectedElements[0] as MainMenuUIController;
        _hudManager = _injectedElements[1] as HUDMenuScreen;
        _player = _injectedElements[2] as PlayerController;
        _levelManager = _injectedElements[3] as LevelManager;
    }

    public void RegisterInjectable()
    {
        InterfaceDependencyInjector.Instance.RegisterInjectable(this);
    }
}