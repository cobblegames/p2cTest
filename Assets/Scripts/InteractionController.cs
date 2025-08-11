using System.Collections;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactionLayer;
    public Camera playerCamera;
    [SerializeField] private bool gameStarted = false;
    [SerializeField] private bool isPaused = false;
    [SerializeField] private IInteractable currentTarget; // Track the currently targeted interactable 

    private void OnEnable()
    {
        GameEvents.OnChangeGameState += HandleGameStateChange;
    }

    private void OnDisable()
    {
        GameEvents.OnChangeGameState -= HandleGameStateChange;
    }

   
    private void HandleGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                gameStarted = false;
                StopAllCoroutines();
                break;

            case GameState.Winning:
                gameStarted = false;
                StopAllCoroutines();
                break;

            case GameState.Losing:
                gameStarted = false;
                StopAllCoroutines();
                break;

            case GameState.InGame:
                isPaused = false;
                if (!gameStarted)
                {
                    StartCoroutine(ProbeInteractionRoutine());
                }
                break;

            case GameState.RadialMenu:
                isPaused = true;
                break;

            default:
                Debug.LogWarning($"Unhandled game state: {state}");
                break;
        }
    }

    private IEnumerator ProbeInteractionRoutine()
    {
        gameStarted = true;
        while (gameStarted)
        {
            if (!isPaused)
            {
                Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();
                    if (interactable != null)
                    {
                        if (currentTarget != interactable)
                        {
                            // New interactable found
                            if (currentTarget != null)
                            {
                                // Clear previous target's crosshair state
                                GameEvents.PostOnInteractableInRage(CrosshairState.Default);
                            }
                            currentTarget = interactable;
                            interactable.Target();
                            interactable = null;
                        }
                    }
                    else if (currentTarget != null)
                    {
                        // No interactable found but we had a previous target
                        GameEvents.PostOnInteractableInRage(CrosshairState.Default);
                        currentTarget = null;
                    }
                }
                else if (currentTarget != null)
                {
                    // Nothing hit but we had a previous target
                    GameEvents.PostOnInteractableInRage(CrosshairState.Default);
                    currentTarget = null;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void TryInteract(PlayerController player)
    {
        if (currentTarget != null)
        {
            GameEvents.PostOnInteractableInRage(CrosshairState.Interacting);
            currentTarget.Interact(player);
            GameEvents.PostOnInteractableInRage(CrosshairState.Default);
            currentTarget = null; // Clear reference after interaction
        }
        else
        {
            if (player.CurrentTheftObject != null && player.PlayerAction == PlayerAction.Drop)
            {
                GameEvents.PostOnInteractableInRage(CrosshairState.Default);
                player.CurrentTheftObject.Drop();
                player.UnregisterTheftObject();
            }
        }
    }
}