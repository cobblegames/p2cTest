/*
    LevelManager is responsible for managing the game level, including tracking collected objects,
    total game objects, and the game timer.
    It checks for win and loss conditions based on the collected objects and game duration.
*/

using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class LevelManager : MonoBehaviour, IInjectable
{
    private HUDMenuScreen _hudManager;

    private int collectedTheftObjectsCount = 0;
    private int totalGameObjectsCount = 0;

    [SerializeField] private float gameDuration = 300f; // Game duration in seconds
    private float currentGameTime = 0f;

    public int TotalGameObjectsCount => totalGameObjectsCount;
    public int CollectedTheftObjectsCount => collectedTheftObjectsCount;
    public float GameDuration => gameDuration;
    public float CurrentGameTime => gameDuration - currentGameTime;

    private Coroutine timer;

    private void OnEnable()
    {
        GameEvents.OnRegisterInjectables += RegisterInjectable;
    }

    private void OnDisable()
    {
        GameEvents.OnRegisterInjectables -= RegisterInjectable;
        UnregisterEvents();
    }

    public void RegisterInjectable()
    {
        InterfaceDependencyInjector.Instance.RegisterInjectable(this);
    }

    public void Initialize(IInjectable[] _injectedElements)
    {
        _hudManager = _injectedElements[0] as HUDMenuScreen;

        RegisterEvents();
    }

    private void RegisterEvents()
    {
        GameEvents.OnGameStart += Handle_StartGame;
        GameEvents.OnGameLost += Handle_LostGame;
        GameEvents.OnGameWon += Handle_WinGame;
    }

    private void UnregisterEvents()
    {
        GameEvents.OnGameStart -= Handle_StartGame;
        GameEvents.OnGameLost -= Handle_LostGame;
        GameEvents.OnGameWon -= Handle_WinGame;
    }

    private void Handle_LostGame()
    {
        StopCoroutine(timer);
    }

    private void Handle_WinGame()
    {
        StopCoroutine(timer);
    }

    private void Handle_StartGame()
    {
        timer = StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (currentGameTime < gameDuration)
        {
            currentGameTime += 1;

            _hudManager.UpdateGameUI();
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Game Over! Time's up!");
        GameEvents.PostOnGameLost();
    }
}