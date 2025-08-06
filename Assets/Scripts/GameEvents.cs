using UnityEngine;

public static class GameEvents 
{
    public delegate void PLAYER_DETECTED(bool isDetected);
    public static event PLAYER_DETECTED OnPlayerDetected;
    public static void PostOnPlayerDetected(bool isDetected)
    {
        if (OnPlayerDetected != null)
            OnPlayerDetected(isDetected);
    }


    public delegate void GAME_START();
    public static event GAME_START OnGameStart;
    public static void PostOnGameStart()
    {
        if (OnGameStart != null)
            OnGameStart();
    }



}
