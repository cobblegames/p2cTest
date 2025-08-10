using UnityEngine;
using System.Collections.Generic;
using Utils;
using System.Collections;
using System;
public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] MainMenuUIController mainMenuUIController;
    [SerializeField] GameplayUIManager gameplayUIManager;

    GameState gameState = GameState.MainMenu;
    public GameState CurrentGameState => gameState;

    List<TheftObject> theftObjects = new List<TheftObject>();
    int collectedTheftObjectsCount = 0;
    public int CollectedTheftObjectsCount => collectedTheftObjectsCount;
    int totalGameObjectsCount = 0;
    public int TotalGameObjectsCount => totalGameObjectsCount;
    PlayerController player;
    public PlayerController Player => player;

    [SerializeField]  float gameDuration = 300f; // Game duration in seconds
    public float GameDuration => gameDuration;
    float currentGameTime = 0f;
    public float CurrentGameTime => gameDuration - currentGameTime;

    Coroutine timer;

    private void OnEnable()
    {
        GameEvents.OnGameStart += StartGame;
        GameEvents.OnGameEnd += EndGame;
    }

    private void OnDisable()
    {
        GameEvents.OnGameStart -= StartGame;
        GameEvents.OnGameEnd -= EndGame;
    }


    private void EndGame()
    {
        StopCoroutine(timer);
    }

    private void StartGame()
    {
        gameState = GameState.InGame;
      
        gameplayUIManager.gameObject.SetActive(true);
        timer =  StartCoroutine(GameTimer());
    }

    public void RegisterTheftObject(TheftObject theftObject)
    {
        if (!theftObjects.Contains(theftObject))
        {
            theftObjects.Add(theftObject);
            totalGameObjectsCount++;
        }
    }


    public void UnregisterTheftObject(TheftObject theftObject)
    {
        if (theftObjects.Contains(theftObject))
        {
            theftObjects.Remove(theftObject);
            collectedTheftObjectsCount++;
            GameEvents.PostOnUpdateGameUI();
        }

        if (collectedTheftObjectsCount >= totalGameObjectsCount)
        {
            Debug.Log("All theft objects collected!");
            GameEvents.PostOnGameWon();
        }
    }

    public void RegisterPlayer(PlayerController playerController)
    {
        if (player == null)
        {
            player = playerController;
            GameEvents.PostOnUpdateGameUI();
        }
        else
        {
            Debug.LogWarning("Player is already registered!");
        }
    }

   IEnumerator GameTimer ()
    {
        while (currentGameTime < gameDuration)
        {
            currentGameTime += 1;
            GameEvents.PostOnUpdateGameUI();
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Game Over! Time's up!");
        GameEvents.PostOnGameLost();
    }
   


}
