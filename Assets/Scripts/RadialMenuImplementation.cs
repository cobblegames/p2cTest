using System.Collections.Generic;
using UnityEngine;

public class RadialMenuImplementation : MonoBehaviour
{
    public RadialMenu radialMenu;

    private void Start()
    {
        // Example actions
        var actions = new List<RadialMenu.MenuAction>
        {
            new RadialMenu.MenuAction
            {
                name = "Pickup",
               
                action = () => Debug.Log("Pickup action!")
            },
            new RadialMenu.MenuAction
            {
                name = "Drop",
               
                action = () => Debug.Log("Drop action!")
            },
            new RadialMenu.MenuAction
            {
                name = "Use",
               
                action = () => Debug.Log("Use Item action!")
            },
         
        };

        radialMenu.CreateMenu(actions);
    }
}