using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    [SerializeField] private int MoveSpeed;
    [SerializeField] private Rigidbody PlayerRigidbody = null;

    private Camera mainCamera = null;
    private Vector2 playerTurnDirection = Vector2.zero;
    private Vector3 playerMoveDirection = Vector3.zero;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        MoveThePlayer();
    }

    public void UpdateTurnDirection(Vector2 newTurnDriection)
    {
        playerTurnDirection = newTurnDriection;
    }

    public void UpdateMovementDirection(Vector3 newMoveDirection)
    {
        playerMoveDirection = newMoveDirection;
    }

    private void MoveThePlayer()
    {
        Vector3 movement = CalculateCameraDirection(playerMoveDirection) * MoveSpeed * Time.deltaTime;
        PlayerRigidbody.MovePosition(movement + transform.position);
    }

    private void TurnThePlayer()
    {

    }

    private Vector3 CalculateCameraDirection(Vector3 moveDirection)
    {
        var cameraForward = mainCamera.transform.forward;
        var cameraRight = mainCamera.transform.right;
        
        cameraForward.y = 0;
        cameraRight.y = 0;

        return cameraForward * moveDirection.z + cameraRight * moveDirection.x;
    }
}
