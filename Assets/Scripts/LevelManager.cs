/*
    LevelManager is responsible for managing the game level, including tracking collected objects,
    total game objects, and the game timer.
    It checks for win and loss conditions based on the collected objects and game duration.
*/

using UnityEngine;
using System.Collections;


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
        GameEvents.OnChangeGameState += Handle_GameStateChange;
    }

  

    private void OnDisable()
    {

        GameEvents.OnChangeGameState -= Handle_GameStateChange;
    }

 
    public void Initialize(IInjectable[] _injectedElements)
    {
        _hudManager = _injectedElements[0] as HUDMenuScreen;
    }


    private void Handle_GameStateChange(GameState _gameState)
    {
        switch(_gameState)
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

    private void Handle_MainMenu()
    {
        if (timer != null)
            StopCoroutine(timer);


        currentGameTime = 0f;
        if(_hudManager != null)
             _hudManager.UpdateGameUI();
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
      
    }
}