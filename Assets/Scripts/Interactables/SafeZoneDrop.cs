using System.Collections;
using UnityEngine;

public class SafeZoneDrop : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerAction assignedAction;
    [SerializeField] Transform dropPoint;
    public void Interact(PlayerController player)
    {
        if (player.PlayerAction == assignedAction)
        {
            if (player.CurrentTheftObject != null)
            {

                StartCoroutine(MoveTheftObjectToDropPoint(player.CurrentTheftObject, 5f));
                
            }

        }
        else
        {
            Debug.LogWarning($"Player {player.name} cannot drop here, action mismatch with {assignedAction}");
        }

    }

    IEnumerator MoveTheftObjectToDropPoint(TheftObject theftObject, float speed)
    {
        //Randomize the drop point position slightly to avoid exact same drop every time
        Vector3 randomizedDropPosition = dropPoint.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));

        while (Vector3.Distance(theftObject.transform.position, randomizedDropPosition) > 0.1f)
        {
            theftObject.transform.position = Vector3.MoveTowards(theftObject.transform.position, randomizedDropPosition, speed * Time.deltaTime);
            yield return null;
        }     
        theftObject.transform.SetParent(dropPoint);
        theftObject.Deliver();
    }

}
