using System.Collections;
using UnityEngine;

public class DoorsObject : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerAction assignedAction;
    [SerializeField] private DoorStatus doorStatus = DoorStatus.Closed;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Transform closedPoint;
    [SerializeField] private Transform openPoint;
    [SerializeField] AudioSource doorSource;
    private void OnEnable()
    {
        GameEvents.OnPlayerDetected += Handle_PlayerDetectedState;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerDetected -= Handle_PlayerDetectedState;
    }

    public void Interact(IInjectable _player)
    {
        PlayerController player = _player as PlayerController;

        if (player.PlayerAction == assignedAction)
        {
            Transform targetPosition = doorStatus == DoorStatus.Closed ? openPoint : closedPoint;

            if (!doorSource.isPlaying)
                doorSource.Play();

            StartCoroutine(MoveDoor(targetPosition, 2f));
        }

    
    }


    private void Handle_PlayerDetectedState(bool isDetected)
    {
        if (isDetected)
        {
            StartCoroutine(MoveDoor(closedPoint, 2f));
        }
    }

    private IEnumerator MoveDoor(Transform targetPosition, float duration)
    {
        Vector3 startPosition = doorTransform.position;
        float elapsedTime = 0f;
       
        while (elapsedTime < duration)
        {
            doorTransform.position = Vector3.Lerp(startPosition, targetPosition.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorStatus = doorStatus == DoorStatus.Closed ? DoorStatus.Open : DoorStatus.Closed; // Toggle door status
        doorTransform.position = targetPosition.position; // Ensure final position is set
    }

    public void Target()
    {
        GameEvents.PostOnInteractableInRage(CrosshairState.TargetingUse);
    }
}