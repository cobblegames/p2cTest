/* it should handle the menu screen functionality
    Currently, it is an empty class that inherits from MenuElement.
    It listens to game state changes but does not implement any specific behavior.
    This is overideable functionality for derived classes.

    But it can be extended later to add specific functionality for the menu screen.
    Like movement, animations, or other UI interactions.
 */

public class MenuScreen : MenuElement
{
  

    protected virtual void Handle_GameStateChange(GameState _gameState)
    { }
}