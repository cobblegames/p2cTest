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

    [SerializeField] private Color detectedColor = Color.red;
    [SerializeField] private Color safeColor = Color.green;

    private PlayerController _player;
    private LevelManager _levelManager;

    public void Initialize(IInjectable[] _injectedElements)
    {
        _player = _injectedElements[0] as PlayerController;
        _levelManager = _injectedElements[1] as LevelManager;
        if (_player == null || _levelManager == null)
        {
            Debug.LogError("HUDMenuScreen: Failed to initialize dependencies.");
            return;
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
        playerStatusValue.text = _player.PlayerStatus.ToString();
        if (_player.PlayerStatus == PlayerAlarmStatus.Detected)
        {
        }
        else
        {
        }

        scoreValue.text = $"{_levelManager.CollectedTheftObjectsCount}/{_levelManager.TotalGameObjectsCount}";
        timerValue.text = $"{Mathf.FloorToInt(_levelManager.CurrentGameTime / 60)}:{Mathf.FloorToInt(_levelManager.CurrentGameTime % 60):00}";
    }
}