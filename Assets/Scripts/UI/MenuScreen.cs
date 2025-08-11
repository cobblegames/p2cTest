/* it should handle the menu screen functionality 
    Currently, it is an empty class that inherits from MenuElement.
    It listens to game state changes but does not implement any specific behavior.
    This is overideable functionality for derived classes.

    But it can be extended later to add specific functionality for the menu screen.
    Like movement, animations, or other UI interactions.
 */

using UnityEngine;

public class MenuScreen : MenuElement
{

    protected virtual void OnEnable()
    {
        GameEvents.OnChangeGameState += Handle_GameStateChange;   
    }


    protected virtual void OnDisable()
    {
        GameEvents.OnChangeGameState -= Handle_GameStateChange;
    }

    protected virtual void Handle_GameStateChange(GameState _gameState){}

}
