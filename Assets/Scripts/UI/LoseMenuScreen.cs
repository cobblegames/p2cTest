using UnityEngine;

public class LoseMenuScreen : MenuScreen
{
    private void OnEnable()
    {
        GameEvents.OnChangeGameState += Handle_GameStateChange;
    }

    private void OnDisable()
    {
        GameEvents.OnChangeGameState -= Handle_GameStateChange;
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
                SwitchMenuState(MenuScreenState.Hidden);
                break;

            case GameState.Losing:
                SwitchMenuState(MenuScreenState.Shown);
                break;

            default:
                Debug.LogWarning($"WinMenuScreen: Unhandled game state {_gameState}");
                break;
        }
    }
}