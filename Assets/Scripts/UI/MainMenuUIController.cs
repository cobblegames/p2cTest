
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MenuScreen, IInjectable
{
    [Header ("Local References")]
    [SerializeField] MenuElement startButton;
    [SerializeField] MenuElement exitGameButton;
  
 
   
    public void Initialize(IInjectable[] _injectedElements)
    {
        
    }


    protected override void Handle_GameStateChange(GameState _gameState)
    {
        base.Handle_GameStateChange(_gameState);

        switch (_gameState)
        {
            case GameState.MainMenu:
                SwitchMenuState(MenuScreenState.Shown);
                break;

            case GameState.InGame:
                SwitchMenuState(MenuScreenState.Hidden);
                break;

            case GameState.Winning:
                SwitchMenuState(MenuScreenState.Hidden);
                break;

            case GameState.Losing:
                SwitchMenuState(MenuScreenState.Hidden);
                break;

            default:
                Debug.LogWarning($"HUDMenuScreen: Unhandled game state {_gameState}");
                break;
        }
    }


}

