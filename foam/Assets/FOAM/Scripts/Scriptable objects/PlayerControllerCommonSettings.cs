using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerSettings", menuName = "Configs")]
public class PlayerControllerCommonSettings : ScriptableObject
{
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float SpeedChangeRate = 10.0f;
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    public float JumpTimeout = 0.50f;
    public float FallTimeout = 0.15f;
}
