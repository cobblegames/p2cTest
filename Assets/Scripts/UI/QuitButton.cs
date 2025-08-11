using UnityEngine;

public class QuitButton : MenuElement
{
   protected override void Handle_OnClick()
    {
        base.Handle_OnClick();
        Debug.Log("Quit Button Clicked");
        Application.Quit();
    }
}
