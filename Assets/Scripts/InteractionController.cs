using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public float interactionDistance = 3f;   
    public LayerMask interactionLayer;
    public Camera playerCamera;

   
    public void TryInteract(PlayerController player)
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(player);
                interactable = null; // Clear reference after interaction
            }           
        }
        else
        {

            if (player.CurrentTheftObject != null && player.PlayerAction == PlayerAction.Drop)
            {
                player.CurrentTheftObject.Drop();
                player.UnregisterTheftObject();
            }
        }
    }
}