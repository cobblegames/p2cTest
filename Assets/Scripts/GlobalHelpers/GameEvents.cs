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

    public static event System.Action OnGameEnd;
    public static void PostOnGameEnd()
    {
        if (OnGameEnd != null)
            OnGameEnd();
    }

    public static event System.Action OnGameWon;
    public static void PostOnGameWon()
    {
        if (OnGameWon != null)
            OnGameWon();
    }

    public static event System.Action OnGameLost;
    public static void PostOnGameLost()
    {
        if (OnGameLost != null)
            OnGameLost();
    }


    public static event System.Action<PlayerAction> OnChangePlayerAction;
    public static void PostOnChangePlayerAction(PlayerAction action)
    {
        if (OnChangePlayerAction != null)
            OnChangePlayerAction(action);
    }

    public static event System.Action OnUpdateGameUI;
    public static void PostOnUpdateGameUI()
    {
        if (OnUpdateGameUI != null)
            OnUpdateGameUI();
    }





}
