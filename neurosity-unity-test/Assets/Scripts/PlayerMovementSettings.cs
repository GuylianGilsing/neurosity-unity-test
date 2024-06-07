using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "ScriptableObjects/PlayerMovementSettings", order = 1)]
public class PlayerMovementSettings : ScriptableObject
{
    [Header("Horizontal movement")]
    public float moveSpeed = 5f;
    public AnimationCurve normalAcceleration;
    public AnimationCurve normalDeceleration;

    [Header("Vertical movement")]
    public float gravity = 9.81f;
    public float gravityMultiplier = 5.0f;
    public float jumpSpeed = 3.5f;

    [Header("Misc")]
    public float turnSmoothTime = 0.1f;
}
