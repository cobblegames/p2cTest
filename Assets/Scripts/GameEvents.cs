using UnityEngine;

public static class GameEvents 
{
    public delegate void PLAYER_DETECTED(bool isDetected, int cameraID);
    public static event PLAYER_DETECTED OnPlayerDetected;
    public static void PostOnPlayerDetected(bool isDetected, int cameraID)
    {
        if (OnPlayerDetected != null)
            OnPlayerDetected(isDetected, cameraID);
    }


    public delegate void GAME_START();
    public static event GAME_START OnGameStart;
    public static void PostOnGameStart()
    {
        if (OnGameStart != null)
            OnGameStart();
    }



}
