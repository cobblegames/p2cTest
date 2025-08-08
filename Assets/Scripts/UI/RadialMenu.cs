using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadialMenu : MonoBehaviour
{
    [Header("Settings")]
    public float radius = 200f;

    public float segmentThickness = 50f;

    //  public KeyCode activationKey = KeyCode.Q;
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);

    public Color highlightColor = new Color(0.4f, 0.4f, 0.8f, 0.8f);
    public Color textColor = Color.white;
    public int textFontSize = 20;

    [Header("References")]
    public Transform segmentContainer;

    public Transform centerPoint;
    public Text selectionText;

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

    private void OnEnable()
    {
        InputManager.Instance.OnShowRadialMenu += ActivateMenu;
        InputManager.Instance.OnHideRadialMenu += DeactivateMenu;
        InputManager.Instance.OnUseAction += ExecuteAction;
        InputManager.Instance.OnLook += HandleSelection;
    }

    private void OnDisable()
    {
        if (InputManager.Instance)
        {
            InputManager.Instance.OnShowRadialMenu -= ActivateMenu;
            InputManager.Instance.OnHideRadialMenu -= DeactivateMenu;
            InputManager.Instance.OnUseAction -= ExecuteAction;
            InputManager.Instance.OnLook -= HandleSelection;

        }
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
            rectTransform.sizeDelta = new Vector2(radius * 2, radius * 2);
            segmentObj.AddComponent<CanvasRenderer>();

            // Add UIMeshSegment component
            UIMeshSegment segmentVisual = segmentObj.AddComponent<UIMeshSegment>();
            segmentVisual.color = normalColor;
            segmentVisual.UpdateSegment(
                currentDegree,
                currentDegree + segmentDegree,
                radius - segmentThickness,
                radius
            );

            // Add text label
            GameObject textObj = new GameObject("Label");
            textObj.transform.SetParent(segmentObj.transform, false);

            Text textComponent = textObj.AddComponent<Text>();
            textComponent.text = actions[i].name;
            textComponent.color = textColor;
            textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComponent.fontSize = textFontSize;
            textComponent.alignment = TextAnchor.MiddleCenter;

            // Position the text (no rotation needed)
            float midAngle = currentDegree + segmentDegree * 0.5f;
            float textDistance = radius - (segmentThickness / 2f);
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
        isActive = true;
        menuRoot.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void ExecuteAction()
    {
        if (currentSelection >= 0 && currentSelection < segments.Count)
        {
            segments[currentSelection].ExecuteAction();
        }
    }

    private void DeactivateMenu()
    {
        isActive = false;
        menuRoot.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        currentSelection = -1;
        UpdateSelectionDisplay();
    }

    private void HandleSelection(Vector2 cursorPosition)
    {
        if (!isActive || segments.Count == 0)
        {
            return;
        }

        Vector2 mousePosition = cursorPosition;
        Vector2 centerPosition = centerPoint.position;
        Vector2 direction = (mousePosition - centerPosition).normalized;

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
        float midRadius = radius - (segmentThickness / 2f);
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

    public void HighlightSegment(int index)
    {
        if (currentSelection >= 0 && currentSelection < segments.Count)
        {
            segments[currentSelection].SetHighlight(false);
        }

        currentSelection = index;

        if (currentSelection >= 0 && currentSelection < segments.Count)
        {
            segments[currentSelection].SetHighlight(true);
        }

        UpdateSelectionDisplay();
    }
}