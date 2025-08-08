using UnityEngine;
[CreateAssetMenu(fileName = "PlayerData", menuName = "Settings/PlayerSettings", order = 100)]
public class PlayerData : ScriptableObject
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpHeight = 1.5f;
    [SerializeField] float gravity = -9.81f;

    public float MoveSpeed => moveSpeed;
    public float JumpHeight => jumpHeight;
    public float Gravity => gravity;
}
