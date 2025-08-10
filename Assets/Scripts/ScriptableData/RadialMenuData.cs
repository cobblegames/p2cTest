using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RadialMenuData", menuName = "Settings/RadialMenuData", order = 100)]
public class RadialMenuData : ScriptableObject
{
    [SerializeField] private float radius = 200f;
    [SerializeField] private float segmentThickness = 50f;
    [SerializeField] private Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    [SerializeField] private Color highlightColor = new Color(0.4f, 0.4f, 0.8f, 0.8f);
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private int textFontSize = 20;
    [SerializeField] private List<MenuActionData> menuActions;

    public float Radius => radius;
    public float SegmentThickness => segmentThickness;
    public Color NormalColor => normalColor;
    public Color HighlightColor => highlightColor;
    public Color TextColor => textColor;
    public int TextFontSize => textFontSize;  
    public List<MenuActionData> MenuActions => menuActions;

}
