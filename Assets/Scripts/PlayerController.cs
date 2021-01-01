using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Behaviours")]
    [SerializeField] private PlayerCameraBehaviour CameraBehaviour = null;
    [SerializeField] private PlayerMovementBehaviour MovementBehaviour = null;

    private Vector3 inputMovement = Vector3.zero;
    private Vector2 inputTurn = Vector2.zero;

    private void Update()
    {
        CameraBehaviour.UpdateRotateDirection(inputTurn);
        MovementBehaviour.UpdateMovementDirection(inputMovement);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        var movement = value.ReadValue<Vector2>();
        inputMovement.x = movement.x;
        inputMovement.z = movement.y;
    }

    public void OnLook(InputAction.CallbackContext value)
    {
        var turn = value.ReadValue<Vector2>();
        inputTurn.x = -turn.y;
        inputTurn.y = turn.x;
    }
}
