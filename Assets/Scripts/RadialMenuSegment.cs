using UnityEngine;
using UnityEngine.UI;

public class RadialMenuSegment : MonoBehaviour
{
    private int index;
    private RadialMenu.MenuAction action;
    private RadialMenu menu;
    private Image image;

    public RadialMenu.MenuAction actionData => action;

    public void Initialize(int index, RadialMenu.MenuAction action, RadialMenu menu)
    {
        this.index = index;
        this.action = action;
        this.menu = menu;
        image = GetComponent<Image>();
    }

    public void SetHighlight(bool highlight)
    {
        image.color = highlight ? menu.highlightColor : menu.normalColor;
    }

    public void ExecuteAction()
    {
        action?.action?.Invoke();
    }
}