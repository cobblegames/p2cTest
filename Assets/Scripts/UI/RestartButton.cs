public class RestartButton : MenuElement
{
    protected override void Handle_OnClick()
    {
        base.Handle_OnClick();
        GameEvents.PostOnRestartGame();
    }
}