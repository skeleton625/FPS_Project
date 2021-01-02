using UnityEngine;
using UnityEngine.InputSystem;
using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class PlayerController : MonoBehaviour
{
    [Header("Player Behaviours")]
    [SerializeField] private PlayerCameraBehaviour CameraBehaviour = null;
    [SerializeField] private PlayerMovementBehaviour MovementBehaviour = null;

    private Vector2 inputTurn = Vector2.zero;
    private Vector3 inputMovement = Vector3.zero;

    private void Update()
    {
        CameraBehaviour.UpdateRotateDirection(inputTurn);
        MovementBehaviour.UpdateMovementDirection(inputMovement);
    }

    public void OnMovement(InputContext value)
    {
        var movement = value.ReadValue<Vector2>();
        inputMovement.x = movement.x;
        inputMovement.z = movement.y;
    }

    public void OnLook(InputContext value)
    {
        var turn = value.ReadValue<Vector2>();
        inputTurn.x = -turn.y;
        inputTurn.y = turn.x;
    }

    public void OnAiming(InputContext value)
    {
        if(value.started)
        {
            CameraBehaviour.UpdateAimingState(true);
        }
        if(value.canceled)
        {
            CameraBehaviour.UpdateAimingState(false);
        }
    }

    public void OnAimingShift(InputContext value)
    {
        if (value.started)
        {
            CameraBehaviour.UpdateAimingPosition();
        }
    }
}
