using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuElement : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler

{
    [SerializeField] protected MenuStateProperties[] _menuStateProperties;
    [SerializeField] protected MenuScreenState currentMenuState = MenuScreenState.Hidden;
    [SerializeField] protected RectTransform rect;
    [SerializeField] protected Image controlImage;
    [SerializeField] protected CanvasGroup canvasGroup;
    public RectTransform Rect => rect;
    public Image ControlledImage => controlImage;
    public CanvasGroup CanvasGroup => canvasGroup;

    protected MenuStateProperties GetMenuStateProperties(MenuScreenState _menuState)
    {
        for (int i = 0; i < _menuStateProperties.Length; i++)
        {
            if (_menuState == _menuStateProperties[i]._state)
            {
                return _menuStateProperties[i];
            }
        }

        return null;
    }

    public void HandleMenuState(MenuScreenState menuState)
    {
        //   Debug.Log("Starting Menu Transition");
        SwitchMenuState(menuState);
    }

    protected void SwitchMenuState(MenuScreenState _newMenuState)
    {
        if (_newMenuState == currentMenuState)
        {
            Debug.Log(gameObject.name + " no need to change the state");
            return;
        }
        else
        {
            Debug.Log(gameObject.name + " changing state to " + currentMenuState.ToString());
            currentMenuState = _newMenuState;
        }

        switch (currentMenuState)
        {
            case MenuScreenState.Shown: Handle_OnShow(); break;
            case MenuScreenState.Hidden: Handle_OnHide(); break;
            case MenuScreenState.Selected: Handle_OnSelected(); break;
            case MenuScreenState.Deselected: Handle_OnDeselected(); break;
            case MenuScreenState.Clicked: Handle_OnClick(); break;
            case MenuScreenState.FirstRun: Handle_OnFirstRun(); break;
            default:
                currentMenuState = MenuScreenState.Error;
                Debug.LogError("State not found"); break;
        }

        if (currentMenuState != MenuScreenState.Error)
        {
            MenuStateProperties properties = GetMenuStateProperties(currentMenuState);

            if (properties != null)
            {
                properties.SetMenuObjectState(this);
            }
        }
    }

    protected virtual void Handle_OnShow()
    { }

    protected virtual void Handle_OnHide()
    { }

    protected virtual void Handle_OnSelected()
    { }

    protected virtual void Handle_OnDeselected()
    { }

    protected virtual void Handle_OnClick()
    { }

    protected virtual void Handle_OnFirstRun()
    { }

    public IEnumerator ChangeScale(RectTransform menuRect, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = menuRect.localScale;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            menuRect.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        menuRect.localScale = targetScale;
        GameEvents.PostOnFinishMenuTransition();
    }

    public IEnumerator ChangeAlpha(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float initialAlpha = canvasGroup.alpha;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        canvasGroup.alpha = targetAlpha;
        GameEvents.PostOnFinishMenuTransition();
    }

    public IEnumerator ChangeImageColor(Image controlImage, Color targetColor, float duration)
    {
        Color initialColor = controlImage.color;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            controlImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        controlImage.color = targetColor;
        GameEvents.PostOnFinishMenuTransition();
    }

    public IEnumerator ChangePosition(RectTransform menuRect, Vector2 targetPosition, float duration)
    {
        Vector2 initialPosition = menuRect.anchoredPosition;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            menuRect.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        menuRect.anchoredPosition = targetPosition;
        GameEvents.PostOnFinishMenuTransition();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Menu Element Clicked: " + gameObject.name);
        HandleMenuState(MenuScreenState.Clicked);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered: " + gameObject.name);
        HandleMenuState(MenuScreenState.Selected);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited: " + gameObject.name);
        HandleMenuState(MenuScreenState.Deselected);
    }
}