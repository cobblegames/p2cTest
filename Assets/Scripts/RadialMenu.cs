using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadialMenu : MonoBehaviour
{
    [Header("Settings")]
    public float radius = 200f;
    public float segmentThickness = 50f;
    public KeyCode activationKey = KeyCode.Q;
    public Color normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    public Color highlightColor = new Color(0.4f, 0.4f, 0.8f, 0.8f);

    [Header("References")]
    public Transform segmentContainer;
    public Transform centerPoint;
    public Text selectionText;

    private List<RadialMenuSegment> segments = new List<RadialMenuSegment>();
    private bool isActive = false;
    private int currentSelection = -1;

    [SerializeField] GameObject menuRoot;

    [System.Serializable]
    public class MenuAction
    {
        public string name;
        public System.Action action;
    }

    public void CreateMenu(List<MenuAction> actions)
    {
        ClearMenu();

        float segmentDegree = 360f / actions.Count;
        float currentDegree = 0f;

        for (int i = 0; i < actions.Count; i++)
        {
            GameObject segmentObj = new GameObject("Segment_" + i);
            segmentObj.transform.SetParent(segmentContainer, false);

            Image segmentImage = segmentObj.AddComponent<Image>();
            segmentImage.type = Image.Type.Filled;
            segmentImage.fillMethod = Image.FillMethod.Radial360;
            segmentImage.fillOrigin = 0;
            segmentImage.fillAmount = 1f / actions.Count;
            segmentImage.color = normalColor;


            // Position and rotate the segment
            segmentObj.transform.localRotation = Quaternion.Euler(0, 0, currentDegree);
            currentDegree += segmentDegree;

            // Position the icon
            float iconDistance = radius - (segmentThickness / 2f);
 
            // Add segment component
            RadialMenuSegment segment = segmentObj.AddComponent<RadialMenuSegment>();
            segment.Initialize(i, actions[i], this);
            segments.Add(segment);
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

    private void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            ActivateMenu();
        }

        if (Input.GetKeyUp(activationKey))
        {
            DeactivateMenu(true);
        }

        if (isActive)
        {
            HandleSelection();
        }
    }

    private void ActivateMenu()
    {
        isActive = true;
        menuRoot.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void DeactivateMenu(bool executeAction)
    {
        isActive = false;
        menuRoot.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (executeAction && currentSelection >= 0 && currentSelection < segments.Count)
        {
            segments[currentSelection].ExecuteAction();
        }

        currentSelection = -1;
        UpdateSelectionDisplay();
    }

    private void HandleSelection()
    {
        Vector2 mousePosition = Input.mousePosition;
        Vector2 centerPosition = centerPoint.position;
        Vector2 direction = (mousePosition - centerPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
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
        }
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