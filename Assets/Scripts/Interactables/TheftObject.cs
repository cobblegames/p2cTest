using System.Collections;
using UnityEngine;

public class TheftObject : MonoBehaviour, IInteractable
{
    [SerializeField] PlayerAction assignedAction;
    [SerializeField] ObjectStatus objectStatus = ObjectStatus.Static;

    Rigidbody rb;
    Collider objectCollider;
    PlayerController playerReference;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        ChangeStatus(ObjectStatus.Static);

        GameManager.Instance.RegisterTheftObject(this);

    }

    void ChangeStatus(ObjectStatus newStatus)
    {
        objectStatus = newStatus;
        Debug.Log($"Object {gameObject.name} status changed to {objectStatus}");

        switch (objectStatus)
        {
            case ObjectStatus.Static:
                rb.isKinematic = false;
                objectCollider.enabled = true;
                break;
            case ObjectStatus.Moving:
                rb.isKinematic = true;
                objectCollider.enabled = false;
                break;
            case ObjectStatus.Stolen:
                rb.isKinematic = true;
                objectCollider.enabled = false;
                break;

            case ObjectStatus.Safe:
                rb.isKinematic = true;
                objectCollider.enabled = false;
                break;
            default:
                Debug.LogWarning($"Unhandled object status: {objectStatus}");
                break;
        }
    }

    public void Interact(PlayerController player)
    {
       if(player.PlayerAction == assignedAction)
        {
            playerReference = player;
         
            Debug.Log($"Player {playerReference.name} is stealing {gameObject.name}");
        
     
            if(playerReference.CurrentTheftObject==null)
            {
                playerReference.RegisterTheftObject(this);
                ChangeStatus(ObjectStatus.Moving);
                StartCoroutine(MoveTowardsCarryingPoint(playerReference.CarryingPoint, 5f));
            }
            else
                Debug.Log($"{playerReference.name} is already carrying an object: {playerReference.CurrentTheftObject.gameObject.name}");

        }
        else
        {
            Debug.Log($"Player {player.name} cannot steal {gameObject.name} with current action: {player.PlayerAction}");
        }
    }

    IEnumerator MoveTowardsCarryingPoint(Transform carryingPoint, float speed)
    {
        while (Vector3.Distance(transform.position, carryingPoint.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, carryingPoint.position, speed * Time.deltaTime);
            yield return null;
        }
        ChangeStatus(ObjectStatus.Stolen);
        transform.SetParent(carryingPoint);
    }

    public void Drop()
    {
        if (playerReference != null)
        {
            Debug.Log($"Dropping {gameObject.name} from {playerReference.name}");
            ChangeStatus(ObjectStatus.Static);
            transform.SetParent(null);
           
            playerReference = null;
        }
        else
        {
            Debug.LogWarning("No player reference to drop the object.");
        }
    }

    public void Deliver()
    {
        if (playerReference != null)
        {
            playerReference.UnregisterTheftObject();
            GameManager.Instance.UnregisterTheftObject(this);
            ChangeStatus(ObjectStatus.Safe);
        }
    }

    
}
