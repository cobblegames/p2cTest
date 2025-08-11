using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
public class RadialMenu : MonoBehaviour, IInjectable
{

    InputManager _inputManager;



    [Header("Settings")]
    [SerializeField] private RadialMenuData radialMenuData;
    public RadialMenuData RadialMenuData => radialMenuData;

    [Header("References")]
    public Transform segmentContainer;

    public Transform centerPoint;
    public TextMeshProUGUI selectionText;

    [SerializeField] private GameObject menuRoot;

    private List<RadialMenuSegment> segments = new List<RadialMenuSegment>();
    private bool isActive = false;
    private int currentSelection = -1;
   
    [System.Serializable]
    public class MenuAction
    {
        public string name;
        public System.Action action;
    }

   


    public void Initialize(IInjectable[] _injectedElements)
    {
       
        _inputManager = _injectedElements[1] as InputManager;
    

        InitRadialMenu();
        RegisterEvents();
    }


    private void RegisterEvents()
    {
        _inputManager.OnShowRadialMenu += ActivateMenu;
        _inputManager.OnHideRadialMenu += DeactivateMenu;
        _inputManager.OnPointerMove += HandleSelection;
    }

    private void UnregisterEvents()
    {
        _inputManager.OnShowRadialMenu -= ActivateMenu;
        _inputManager.OnHideRadialMenu -= DeactivateMenu;
        _inputManager.OnPointerMove -= HandleSelection;
    }

    private void InitRadialMenu()
    {
        var actions = new List<RadialMenu.MenuAction>();
        foreach (var action in radialMenuData.MenuActions)
        {
            actions.Add(new RadialMenu.MenuAction
            {
                name = action.Action.ToString(),
                action = () =>
                {
                 
                    GameEvents.PostOnChangePlayerAction(action.Action);
                    action.ExecuteAction();
                }
            });
        }

        CreateMenu(actions);
    }


    public void CreateMenu(List<MenuAction> actions)
    {
        ClearMenu();

        if (segmentContainer.GetComponentInParent<Canvas>() == null)
        {
            Debug.LogError("RadialMenu must be a child of a Canvas!");
            return;
        }

        float segmentDegree = 360f / actions.Count;
        float currentDegree = 0f;

        for (int i = 0; i < actions.Count; i++)
        {
            GameObject segmentObj = new GameObject("Segment_" + i);
            segmentObj.transform.SetParent(segmentContainer, false);

            // Add required UI components
            var rectTransform = segmentObj.AddComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero; // Center position
            rectTransform.sizeDelta = new Vector2(radialMenuData.Radius * 2, radialMenuData.Radius * 2);
            segmentObj.AddComponent<CanvasRenderer>();

            // Add UIMeshSegment component
            UIMeshSegment segmentVisual = segmentObj.AddComponent<UIMeshSegment>();
            segmentVisual.color = radialMenuData.NormalColor;
            segmentVisual.UpdateSegment(
                currentDegree,
                currentDegree + segmentDegree,
                radialMenuData.Radius - radialMenuData.SegmentThickness,
                radialMenuData.Radius
            );

            // Add text label
            GameObject textObj = new GameObject("Label");
            textObj.transform.SetParent(segmentObj.transform, false);

            TextMeshProUGUI textComponent = textObj.AddComponent<TextMeshProUGUI>();
            textComponent.text = actions[i].name;
            textComponent.color = radialMenuData.TextColor;
          //  textComponent.font = Resources.Load<TMP_FontAsset>("LiberationSans SDF");
            textComponent.fontSize = radialMenuData.TextFontSize;
            textComponent.alignment = TextAlignmentOptions.MidlineJustified;

            // Position the text (no rotation needed)
            float midAngle = currentDegree + segmentDegree * 0.5f;
            float textDistance = radialMenuData.Radius - (radialMenuData.SegmentThickness / 2f);
            textObj.transform.localPosition = new Vector3(
                Mathf.Sin(midAngle * Mathf.Deg2Rad) * textDistance,
                Mathf.Cos(midAngle * Mathf.Deg2Rad) * textDistance,
                0
            );

            // Add segment component
            RadialMenuSegment segment = segmentObj.AddComponent<RadialMenuSegment>();
            segment.Initialize(i, actions[i], this, segmentVisual);
            segments.Add(segment);

            currentDegree += segmentDegree;
        }
    }

    public void ClearMenu()
    {
        foreach (Transform child in segmentContainer)
        {
            Destroy(child.gameObject);
        }
        segments.Clear();
    }

    private void ActivateMenu()
    {
      
        Debug.Log("Activating Radial Menu");
        isActive = true;
        menuRoot.SetActive(true);
        GameEvents.PostOnChangeGameState(GameState.RadialMenu);
    }

    private void ExecuteAction()
    {
        if (currentSelection >= 0 && currentSelection < segments.Count)
        {
            Debug.Log($"Executing action: {segments[currentSelection].actionData.name}");
            segments[currentSelection].ExecuteAction();
        }
    }

    private void DeactivateMenu()
    {
        ExecuteAction();
        isActive = false;
        menuRoot.SetActive(false);

        currentSelection = -1;
        UpdateSelectionDisplay();

        GameEvents.PostOnChangeGameState(GameState.InGame);

    }

    private void HandleSelection(Vector2 cursorPosition)
    {
        if (!isActive || segments.Count == 0)
        {
            return;
        }

     
        Vector2 centerPosition = centerPoint.position;
        Vector2 direction = (cursorPosition - centerPosition).normalized;

        float angle = Mathf.Atan2(-direction.y, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 360f + 90f) % 360f; // Adjust for Unity's coordinate system

        int newSelection = Mathf.FloorToInt(angle / (360f / segments.Count));
        newSelection = newSelection % segments.Count;

        if (newSelection != currentSelection)
        {
            if (currentSelection >= 0)
            {
                segments[currentSelection].SetHighlight(false);
            }

            currentSelection = newSelection;
            segments[currentSelection].SetHighlight(true);

            UpdateSelectionDisplay();
            UpdateSelectionVisual();
        }
    }

    private void UpdateSelectionVisual()
    {
        if (currentSelection < 0 || currentSelection >= segments.Count)
        {
            return;
        }

        // Get the selected segment's position
        Transform selectedSegment = segments[currentSelection].transform;
        Vector2 segmentCenter = GetSegmentCenter(selectedSegment);

        // Calculate line properties
        float distance = segmentCenter.magnitude;
        float angle = Mathf.Atan2(segmentCenter.y, segmentCenter.x) * Mathf.Rad2Deg;
    }

    private Vector3 GetSegmentCenter(Transform segment)
    {
        // Calculate the center point of the segment at the middle radius
        float segmentAngle = 360f / segments.Count;
        float midRadius = radialMenuData.Radius - (radialMenuData.SegmentThickness / 2f);
        float segmentRotation = segment.localEulerAngles.z;
        float centerAngle = (segmentRotation + segmentAngle / 2f) * Mathf.Deg2Rad;

        return new Vector3(
            Mathf.Sin(centerAngle) * midRadius,
            Mathf.Cos(centerAngle) * midRadius,
            0
        );
    }

    private void UpdateSelectionDisplay()
    {
        if (selectionText != null)
        {
            selectionText.text = (currentSelection >= 0) ?
                segments[currentSelection].actionData.name : "None";
        }
    }

 
}