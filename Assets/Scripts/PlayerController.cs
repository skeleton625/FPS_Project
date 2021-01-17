using UnityEngine;
using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;

public class PlayerController : MonoBehaviour
{
    [Header("Player Behaviours")]
    [SerializeField] private PlayerCameraBehaviour CameraBehaviour = null;
    [SerializeField] private PlayerMovementBehaviour MovementBehaviour = null;

    private bool isAiming = false;
    private bool isShifted = false;
    private Vector2 inputTurn = Vector2.zero;
    private Vector3 inputMovement = Vector3.zero;

    private void Update()
    {
        CameraBehaviour.UpdateRotateDirection(inputTurn);
        //MovementBehaviour.UpdateMovementDirection(inputMovement);
    }

    public void OnMovement(InputContext value)
    {
        var movement = value.ReadValue<Vector2>();
        inputMovement.x = movement.x;
        inputMovement.z = movement.y;
        MovementBehaviour.UpdateMovementDirection(inputMovement);
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
            isAiming = true;
        }
        if(value.canceled)
        {
            isAiming = false;
            isShifted = false;
            CameraBehaviour.UpdateAimingPosition(false);
        }
        CameraBehaviour.UpdateAimingState(isAiming);
        MovementBehaviour.UpdateAimmingState(isAiming);
    }

    public void OnShiftKey(InputContext value)
    {
        if (isAiming)
        {
            if(value.started)
            {
                isShifted = !isShifted;
                CameraBehaviour.UpdateAimingPosition(isShifted);
            }
        }
        else
        {
            if (value.started)
            {
                MovementBehaviour.UpdateRunningState(true);
            }
            else if (value.canceled)
            {
                MovementBehaviour.UpdateRunningState(false);
            }
        }
    }
}
