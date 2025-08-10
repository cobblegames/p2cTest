using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuObjectState", menuName = "Settings/Menu Object State")]
[System.Serializable]
public class MenuObjectData : ScriptableObject
{
    [Header("Menu State Settings ")]
    [SerializeField] private string stateTag;

    [SerializeField] private float _canvasGroupAlpha;
    [SerializeField] private float _duration;
    [SerializeField] private Color _controlledImageColor;
    [SerializeField] private Vector3 _scale;

    public string StateTag => stateTag;
    public float CanvasGroupAlpha => _canvasGroupAlpha;
    public float Duration => _duration;
    public Color ControlledImageColor => _controlledImageColor;
    public Vector3 Scale => _scale;

    [Header("Menu State Flags ")]
    [SerializeField] private bool _changeCanvasGroupAlpha = false;

    [SerializeField] private bool _changeControlledImageColor = false;

    [SerializeField] private bool _changeScale = false;
    public bool ChangeCanvasGroupAlpha => _changeCanvasGroupAlpha;
    public bool ChangeControlledImageColor => _changeControlledImageColor;
    public bool ChangeScale => _changeScale;
}