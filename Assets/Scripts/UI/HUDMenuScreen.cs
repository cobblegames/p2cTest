using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDMenuScreen : MenuScreen, IInjectable
{
    [Header("References - Drag those in inspector")]
    [SerializeField] private TextMeshProUGUI playerStatusValue;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI timerValue;
    [SerializeField] private TextMeshProUGUI currentPlayerAction;

    [SerializeField] private Color detectedColor = Color.red;
    [SerializeField] private Color safeColor = Color.green;

   
    private LevelManager _levelManager;

    protected override void OnEnable()
    {
        base.OnEnable();
        GameEvents.OnChangePlayerAction += ctx =>  currentPlayerAction.text = ctx.ToString() ;
        GameEvents.OnPlayerDetected += Handle_PlayerDetected;
           
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameEvents.OnChangePlayerAction -= ctx => currentPlayerAction.text = ctx.ToString();
        GameEvents.OnPlayerDetected -= Handle_PlayerDetected;
    }


    public void Initialize(IInjectable[] _injectedElements)
    {
        
        _levelManager = _injectedElements[0] as LevelManager;
        if (_levelManager == null)
        {
            Debug.LogError("HUDMenuScreen: Failed to initialize dependencies.");
            return;
        }
    }

    private void Handle_PlayerDetected(bool isDetected)
    {
        if (isDetected)
        {         
            playerStatusValue.color = detectedColor;
            playerStatusValue.text = "Detected";
        }
        else
        {         
            playerStatusValue.color = safeColor;
            playerStatusValue.text = "Safe";
        }
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
                SwitchMenuState(MenuScreenState.Shown);
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

    public void UpdateGameUI()
    {
        scoreValue.text = $"{_levelManager.CollectedTheftObjectsCount}/{_levelManager.TotalGameObjectsCount}";
        timerValue.text = $"{Mathf.FloorToInt(_levelManager.CurrentGameTime / 60)}:{Mathf.FloorToInt(_levelManager.CurrentGameTime % 60):00}";
    }
}