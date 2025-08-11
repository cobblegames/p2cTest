using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CrosshairStateImage
{
    [SerializeField] private CrosshairState _state;
    [SerializeField] private Sprite _sprite;

    public CrosshairState State => _state;
    public Sprite Sprite => _sprite;
}

public class Crosshair : MonoBehaviour
{
    [SerializeField] private CrosshairStateImage[] crosshairStates;
    [SerializeField] private Image image;
    [SerializeField] private CrosshairState currentState = CrosshairState.Default;

    private void OnEnable()
    {
        GameEvents.OnInteractableInRage += SetCrosshairState;
    }

    private void OnDisable()
    {
        GameEvents.OnInteractableInRage -= SetCrosshairState;
    }

    private void SetCrosshairState(CrosshairState state)
    {
        if (currentState == state)
            return; // No change in state, no need to update the crosshair

        currentState = state;
        Sprite crosshairSprite = GetCrosshairSprite(currentState);
        if (crosshairSprite != null)
        {
            image.sprite = crosshairSprite;
        }
        else
        {
            Debug.LogWarning($"Crosshair state {state} does not have a corresponding sprite.");
        }
    }

    private Sprite GetCrosshairSprite(CrosshairState state)
    {
        foreach (var crosshairState in crosshairStates)
        {
            if (crosshairState.State == state)
            {
                return crosshairState.Sprite;
            }
        }
        return null;
    }
}