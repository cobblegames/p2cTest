using System.Collections;
using UnityEngine;

public class SafeZoneDrop : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerAction assignedAction;
    [SerializeField] private Transform dropPoint;

    public void Interact(IInjectable player)
    {
        PlayerController _player = player as PlayerController;

        if (_player.PlayerAction == assignedAction)
        {
            if (_player.CurrentTheftObject != null)
            {
                StartCoroutine(MoveTheftObjectToDropPoint(_player.CurrentTheftObject, 5f));
            }
        }
        else
        {
            Debug.LogWarning($"Player {_player.name} cannot drop here, action mismatch with {assignedAction}");
        }
    }

    private IEnumerator MoveTheftObjectToDropPoint(TheftObject theftObject, float speed)
    {
        //Randomize the drop point position slightly to avoid exact same drop every time
        Vector3 randomizedDropPosition = dropPoint.position + new Vector3(Random.Range(-2f, 2), 0, Random.Range(-2f, 2f));
        theftObject.transform.SetParent(null);
        while (Vector3.Distance(theftObject.transform.position, randomizedDropPosition) > 0.1f)
        {
            theftObject.transform.position = Vector3.MoveTowards(theftObject.transform.position, randomizedDropPosition, speed * Time.deltaTime);
            yield return null;
        }
        theftObject.transform.SetParent(dropPoint);
        theftObject.Deliver();
    }

    public void Target()
    {
        GameEvents.PostOnInteractableInRage(CrosshairState.TargetingDrop);
    }
}