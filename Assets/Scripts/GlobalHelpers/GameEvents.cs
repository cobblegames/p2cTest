public static class GameEvents
{
    public static event System.Action<GameState> OnChangeGameState;

    public static void PostOnChangeGameState(GameState _newState)
    {
        if (OnChangeGameState != null)
            OnChangeGameState(_newState);
    }

    public static event System.Action<bool> OnPlayerDetected;

    public static void PostOnPlayerDetected(bool isDetected)
    {
        if (OnPlayerDetected != null)
            OnPlayerDetected(isDetected);
    }

    public static event System.Action<PlayerAction> OnChangePlayerAction;

    public static void PostOnChangePlayerAction(PlayerAction action)
    {
        if (OnChangePlayerAction != null)
            OnChangePlayerAction(action);
    }

    public static event System.Action OnFinishMenuTransition;

    public static void PostOnFinishMenuTransition()
    {
        if (OnFinishMenuTransition != null)
            OnFinishMenuTransition();
    }

    public static event System.Action<CrosshairState> OnInteractableInRage;

    public static void PostOnInteractableInRage(CrosshairState state)
    {
        if (OnInteractableInRage != null)
            OnInteractableInRage(state);
    }

    public static event System.Action OnRestartGame;

    public static void PostOnRestartGame()
    {
        if (OnRestartGame != null)
            OnRestartGame();
    }
}