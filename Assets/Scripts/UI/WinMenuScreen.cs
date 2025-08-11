using TMPro;
using UnityEngine;

public class WinMenuScreen : MenuScreen, IInjectable
{
    [Header("InjectableReference")]
    private LevelManager _levelManager;

    [Header("Local References")]
    [SerializeField] private TextMeshProUGUI scoreValue;

    public void Initialize(IInjectable[] _injectedElements)
    {
        _levelManager = _injectedElements[0] as LevelManager;
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
                SwitchMenuState(MenuScreenState.Hidden);
                break;

            case GameState.InGame:
                SwitchMenuState(MenuScreenState.Hidden);
                break;

            case GameState.Winning:
                SwitchMenuState(MenuScreenState.Shown);
                break;

            case GameState.Losing:
                SwitchMenuState(MenuScreenState.Hidden);
                break;

            default:
                Debug.LogWarning($"WinMenuScreen: Unhandled game state {_gameState}");
                break;
        }
    }

    protected override void Handle_OnShow()
    {
        base.Handle_OnShow();
        scoreValue.text = _levelManager.CurrentGameTime.ToString();
    }
}