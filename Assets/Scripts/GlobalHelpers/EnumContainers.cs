public enum PlayerAction
{
    Pickup = 0,
    Drop = 1,
    Use = 2,
}


public enum PlayerAlarmStatus 
{ NotDetected = 0, 
    Detected = 1 
}

public enum ObjectStatus
{
    Static = 0,
    Moving = 1,
    Stolen = 2,
    Safe = 3
}

 public enum DoorStatus 
{
    Open = 0,
    Closed = 1,
    Locked = 2
}


public enum GameState
{
    MainMenu = 0,
    InGame = 1,
    Winning = 2,
    Losing = 3,
}

[System.Serializable]
public enum MenuScreenState
{
    Shown = 0,
    Hidden = 1,
    Selected = 2,
    Deselected = 3,
    Error = 4
}
