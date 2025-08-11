/*
    LevelManager is responsible for managing the game level, including tracking collected objects,
    total game objects, and the game timer.
    It checks for win and loss conditions based on the collected objects and game duration.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IInjectable
{
    private HUDMenuScreen _hudManager;

    private int collectedTheftObjectsCount = 0;
    private int totalGameObjectsCount = 0;

    [SerializeField] private float gameDuration = 300f; // Game duration in seconds
    [SerializeField] private float currentGameTime = 0f;

    public int TotalGameObjectsCount => totalGameObjectsCount;
    public int CollectedTheftObjectsCount => collectedTheftObjectsCount;
    public float GameDuration => gameDuration;
    public float CurrentGameTime => gameDuration - currentGameTime;

    private Coroutine timer;

    private List<TheftObject> collectedObjects = new List<TheftObject>();


    void RegisterEvents()
    {
        GameEvents.OnChangeGameState += Handle_GameStateChange;
        GameEvents.OnRestartGame += UnregisterEvents;
        GameEvents.OnCaughtPenalty += Handle_CaughtPenalty;
    }

  

    void UnregisterEvents()
    {
        GameEvents.OnChangeGameState -= Handle_GameStateChange;
        GameEvents.OnCaughtPenalty -= Handle_CaughtPenalty;
        GameEvents.OnRestartGame -= UnregisterEvents;
    }



    public void Initialize(IInjectable[] _injectedElements)
    {
        _hudManager = _injectedElements[0] as HUDMenuScreen;

        collectedObjects.AddRange(Object.FindObjectsByType<TheftObject>(FindObjectsSortMode.None));

        for (int i = 0; i < collectedObjects.Count; i++)
        {
            if (collectedObjects[i] != null)
            {
                totalGameObjectsCount++;
                collectedObjects[i].Initialize(new IInjectable[] { this });
            }
            else
            {
                Debug.LogWarning("LevelManager: Found a null TheftObject in the scene.");
            }
        }

        RegisterEvents();
    }

    private void Handle_GameStateChange(GameState _gameState)
    {
        switch (_gameState)
        {
            case GameState.MainMenu:
                Handle_MainMenu();
                break;

            case GameState.InGame:
                Handle_StartGame();
                break;

            case GameState.Winning:
                Handle_WinGame();
                break;

            case GameState.Losing:
                Handle_LostGame();
                break;

            default:
                Debug.LogWarning($"LevelManager: Unhandled game state {_gameState}");
                break;
        }
    }
    private void Handle_CaughtPenalty()
    {
        if(currentGameTime < gameDuration)
        {
            currentGameTime += 20f;
            if (currentGameTime > gameDuration)
            {
                GameEvents.PostOnChangeGameState(GameState.Losing);
                currentGameTime = gameDuration;
                
            }
            if (_hudManager != null)
                _hudManager.UpdateGameUI();
        }
      
    }
    public void CollectTheftObject(TheftObject theftObject)
    {
        if (theftObject != null && collectedObjects.Contains(theftObject))
        {
            collectedObjects.Remove(theftObject);
            collectedTheftObjectsCount++;
            Debug.Log($"Collected object: {theftObject.name}. Total collected: {collectedTheftObjectsCount}/{totalGameObjectsCount}");
            if (_hudManager != null)
                _hudManager.UpdateGameUI();
            if (collectedTheftObjectsCount >= totalGameObjectsCount)
            {
                GameEvents.PostOnChangeGameState(GameState.Winning);
            }
        }
        else
        {
            Debug.LogWarning("Attempted to collect a null or already collected TheftObject.");
        }
    }

    private void Handle_MainMenu()
    {
        if (timer != null)
            StopCoroutine(timer);

        currentGameTime = 0f;
        if (_hudManager != null)
            _hudManager.UpdateGameUI();
    }

    private void Handle_LostGame()
    {
        StopCoroutine(timer);
        timer = null;
    }

    private void Handle_WinGame()
    {
        StopCoroutine(timer);
        timer = null;
    }

    private void Handle_StartGame()
    {
        if (timer == null)
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
    }
}