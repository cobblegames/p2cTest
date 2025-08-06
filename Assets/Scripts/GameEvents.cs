using UnityEngine;

public static class GameEvents 
{
    public delegate void PLAYER_DETECTED();
    public static event PLAYER_DETECTED OnPlayerDetected;
    public static void PostOnPlayerDetected()
    {
        if (OnPlayerDetected != null)
            OnPlayerDetected();
    }


    public delegate void GAME_START();
    public static event GAME_START OnGameStart;
    public static void PostOnGameStart()
    {
        if (OnGameStart != null)
            OnGameStart();
    }



}
