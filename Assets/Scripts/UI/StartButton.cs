using UnityEngine;

public class StartButton : MenuElement
{
    protected override void Handle_OnClick()
    {
        base.Handle_OnClick();
        Debug.Log("Start Button Clicked");
        GameEvents.PostOnChangeGameState(GameState.InGame);
    }


}
