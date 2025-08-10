using UnityEngine;

[System.Serializable]
public class MenuControlObject
{
    public MenuElement menuElement;
    public MenuScreenState menuElementSetState;

    public void SetControlObjectState()
    {
        if (menuElement != null)
        {
            menuElement.HandleMenuState(menuElementSetState);
        }
        else
        {
            Debug.Log(" Null element");
        }
    }
}

[System.Serializable]
public class MenuStateProperties
{
    [SerializeField] private string stateTag;
    public MenuScreenState _state;

    public MenuObjectData _menuObjectData;
    public MenuControlObject[] _controlObjects;

    public void SetMenuObjectState(MenuElement element)
    {
        if (_menuObjectData.ChangeScale)
        {
            if (element.Rect.localScale != _menuObjectData.Scale)
            {
                element.StartCoroutine(element.ChangeScale(element.Rect, _menuObjectData.Scale, _menuObjectData.Duration));
            }
        }

        if (_menuObjectData.ChangeCanvasGroupAlpha)
        {
            CanvasGroup cg = element.CanvasGroup;
            if (cg != null)
            {
                if (cg.alpha != _menuObjectData.CanvasGroupAlpha)
                {
                    element.StartCoroutine(element.ChangeAlpha(cg, _menuObjectData.CanvasGroupAlpha, _menuObjectData.Duration));
                }
            }
        }

        if(_menuObjectData.ChangeControlledImageColor)
        {
            if (element.ControlledImage != null)
            {
               element.StartCoroutine(element.ChangeImageColor(element.ControlledImage, _menuObjectData.ControlledImageColor, _menuObjectData.Duration));
            }
        }

        for (int i = 0; i < _controlObjects.Length; i++)
        {
            if (_controlObjects[i].menuElement != null)
            {
                _controlObjects[i].SetControlObjectState();
            }
            else
            {
                Debug.Log("Control object is null");
            }
        }
    }
}