using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HUDMenuScreen : MenuScreen, IInjectable
{
    [Header ("References")]
    [SerializeField] private TextMeshProUGUI playerStatusValue;
    [SerializeField] private TextMeshProUGUI scoreValue;
    [SerializeField] private TextMeshProUGUI timerValue;



    [SerializeField] Color detectedColor = Color.red;
    [SerializeField] Color safeColor = Color.green;


    PlayerController _player;
    LevelManager _levelManager;

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
        _player = _injectedElements[0] as PlayerController;
        _levelManager = _injectedElements[1] as LevelManager;
        if (_player == null || _levelManager == null)
        {
            Debug.LogError("HUDMenuScreen: Failed to initialize dependencies.");
            return;
        }
      
    }

    void RegisterEvents()
    {
     
    }

    void UnregisterEvents()
    {
     
    }


    public void RegisterInjectable()
    {
       InterfaceDependencyInjector.Instance.RegisterInjectable(this);
      
    }

    void HandleStartGame()
    {

    }

    void HandleWinGame()
    {
     
    }

    void HandleLoseGame()
    {

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
