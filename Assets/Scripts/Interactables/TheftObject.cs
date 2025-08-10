using System.Collections;
using UnityEngine;

public class TheftObject : MonoBehaviour, IInteractable, IInjectable
{
    [SerializeField] PlayerAction assignedAction;
    [SerializeField] ObjectStatus objectStatus = ObjectStatus.Static;

    Rigidbody rb;
    Collider objectCollider;

    PlayerController _player;
    LevelManager _levelManager;

    void OnEnable()
    {
        GameEvents.OnRegisterInjectables += RegisterInjectable;
    }

    void OnDisable()
    {
        GameEvents.OnRegisterInjectables -= RegisterInjectable;
    }

    public void Initialize(IInjectable[] _injectedElements)
    {
        _levelManager = _injectedElements[0] as LevelManager;
        // We could inject the PlayerController if needed, but for now we just need GameController - PlayerController will be set during interaction
        // In case we may have more than one player - this way we can store only the one who is currently interacting with the object

        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
        ChangeStatus(ObjectStatus.Static);
    }

    public void RegisterInjectable()
    {
       InterfaceDependencyInjector.Instance.RegisterInjectable(this);
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

    public void Interact(IInjectable player)
    {
        _player = player as PlayerController;

        if (_player.PlayerAction == assignedAction)
        {
        
            Debug.Log($"Player {_player.name} is stealing {gameObject.name}");
        
            if(_player.CurrentTheftObject==null)
            {
                _player.RegisterTheftObject(this);
                ChangeStatus(ObjectStatus.Moving);
                StartCoroutine(MoveTowardsCarryingPoint(_player.CarryingPoint, 5f));
            }
            else
                Debug.Log($"{_player.name} is already carrying an object: {_player.CurrentTheftObject.gameObject.name}");

        }
        else
        {
            Debug.Log($"Player {_player.name} cannot steal {gameObject.name} with current action: {_player.PlayerAction}");
        }
    }

    IEnumerator MoveTowardsCarryingPoint(Transform carryingPoint, float speed)
    {
        transform.SetParent(null);

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
        if (_player != null)
        {
            Debug.Log($"Dropping {gameObject.name} from {_player.name}");
            _player.UnregisterTheftObject();
            ChangeStatus(ObjectStatus.Static);
            transform.SetParent(null);
            _player = null;
        }
        else
        {
            Debug.LogWarning("No player reference to drop the object.");
        }
    }

    public void Deliver()
    {
        if (_player != null)
        {
            _player.UnregisterTheftObject();
            _player = null;
            ChangeStatus(ObjectStatus.Safe);

        }
    }

  
}
