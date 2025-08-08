using System;
using UnityEngine;

public static class GameEvents 
{
  
    public static event System.Action<bool> OnPlayerDetected;
    public static void PostOnPlayerDetected(bool isDetected)
    {
        if (OnPlayerDetected != null)
            OnPlayerDetected(isDetected);
    }


    
    public static event System.Action OnGameStart;
    public static void PostOnGameStart()
    {
        if (OnGameStart != null)
            OnGameStart();
    }

    
    public static event System.Action<PlayerAction> OnChangePlayerAction;
    public static void PostOnChangePlayerAction(PlayerAction action)
    {
        if (OnChangePlayerAction != null)
            OnChangePlayerAction(action);
    }

   



}
