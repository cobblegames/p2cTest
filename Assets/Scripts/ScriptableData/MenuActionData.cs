using UnityEngine;
[CreateAssetMenu(fileName = "MenuActionData", menuName = "Settings/MenuAction", order = 100)]

[System.Serializable]
public class MenuActionData : ScriptableObject
{
    [SerializeField] PlayerAction action;
    public PlayerAction Action => action;

    public void ExecuteAction()
    {
        // Additional logic;
        Debug.Log($"Executing action: {action}");
    }

}
