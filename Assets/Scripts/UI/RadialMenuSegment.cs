using UnityEngine;
using UnityEngine.UI;

public class RadialMenuSegment : MonoBehaviour
{
    private int index;
    private RadialMenu.MenuAction action;
    private RadialMenu menu;
    private UIMeshSegment segmentVisual;

    public RadialMenu.MenuAction actionData => action;

    public void Initialize(int index, RadialMenu.MenuAction action, RadialMenu menu, UIMeshSegment visual)
    {
        this.index = index;
        this.action = action;
        this.menu = menu;
        this.segmentVisual = visual;
        gameObject.layer = LayerMask.NameToLayer("UI");
    }

    public void SetHighlight(bool highlight)
    {
        segmentVisual.color = highlight ? menu.highlightColor : menu.normalColor;
        segmentVisual.SetVerticesDirty(); // Update the visual
    }

    public void ExecuteAction()
    {
        action?.action?.Invoke();
    }
}