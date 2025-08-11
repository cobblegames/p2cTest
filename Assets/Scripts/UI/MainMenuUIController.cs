using UnityEngine;

public class MainMenuUIController : MenuScreen, IInjectable
{
    [SerializeField] private Camera _menuCamera;


    public void Initialize(IInjectable[] _injectedElements)
    {
        RegisterEvents();
    }

    void RegisterEvents()
    {
        GameEvents.OnChangeGameState += Handle_GameStateChange;
        GameEvents.OnRestartGame += UnregisterEvents;
    }

    void UnregisterEvents()
    {
        GameEvents.OnChangeGameState -= Handle_GameStateChange;
        GameEvents.OnRestartGame -= UnregisterEvents;
    }


    protected override void Handle_GameStateChange(GameState _gameState)
    {
        base.Handle_GameStateChange(_gameState);

        switch (_gameState)
        {
            case GameState.MainMenu:
                SwitchMenuState(MenuScreenState.Shown);
                _menuCamera.gameObject.SetActive(true);
                break;

            case GameState.InGame:
                SwitchMenuState(MenuScreenState.Hidden);
                _menuCamera.gameObject.SetActive(false);
                break;

            case GameState.Winning:
                SwitchMenuState(MenuScreenState.Hidden);
                _menuCamera.gameObject.SetActive(false);
                break;

            case GameState.Losing:
                SwitchMenuState(MenuScreenState.Hidden);
                _menuCamera.gameObject.SetActive(false);
                break;

            default:
                Debug.LogWarning($"HUDMenuScreen: Unhandled game state {_gameState}");
                break;
        }
    }
}