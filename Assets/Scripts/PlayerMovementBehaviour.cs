using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    [SerializeField] private float DefaultMoveSpeed = 5f;
    [SerializeField] private float DefaultSlowValue = .5f;
    [SerializeField] private float DefaultFastValue = 1.5f;
    [SerializeField] private float DefaultTurnSpeed = .01f;
    [SerializeField] private Transform ModelTransform = null;
    [SerializeField] private Rigidbody PlayerRigidbody = null;

    private bool isAimming = false;
    private bool isRunning = false;
    private float preMoveSpeed = 0f;
    private float preTurnSpeed = 0f;
    private Vector3 moveDirection = Vector3.zero;
    private Transform cameraTransform = null;

    private void Awake()
    {
        preMoveSpeed = DefaultMoveSpeed;
        preTurnSpeed = DefaultTurnSpeed;
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        MoveThePlayer();
        TurnThePlayer();
    }

    private void MoveThePlayer()
    {
        var moveSpeed = preMoveSpeed;
        if (moveDirection.z < 0)
            moveSpeed = DefaultMoveSpeed * DefaultSlowValue;

        Vector3 movement = CalculateCameraDirection(moveDirection) * moveSpeed * Time.deltaTime;
        PlayerRigidbody.MovePosition(movement + transform.position);
    }

    private void TurnThePlayer()
    {
        if (!isAimming && moveDirection.sqrMagnitude > .01f)
        {
            var lookDirection = moveDirection;
            if (lookDirection.z < 0) lookDirection.z *= -1;
            var lookRotation = Quaternion.LookRotation(CalculateCameraDirection(lookDirection));
            var rotation = Quaternion.Slerp(ModelTransform.rotation, lookRotation, preTurnSpeed);
            ModelTransform.rotation = rotation;
        }
    }

    private Vector3 CalculateCameraDirection(Vector3 moveDirection)
    {
        var cameraForward = cameraTransform.forward;
        var cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        return cameraForward * moveDirection.z + cameraRight * moveDirection.x;
    }

    public void UpdateMovementDirection(Vector3 newMoveDirection)
    {
        moveDirection = newMoveDirection;
    }

    public void UpdateRunningState(bool isRunning)
    {
        if(isRunning)
        {
            preMoveSpeed = DefaultMoveSpeed * DefaultFastValue;
        }
        else
        {
            preMoveSpeed = DefaultMoveSpeed;
        }
        this.isRunning = isRunning;
    }

    public void UpdateAimmingState(bool isAimming)
    {
        if(isAimming)
        {
            preMoveSpeed = DefaultMoveSpeed * DefaultSlowValue;
            ModelTransform.rotation = Quaternion.LookRotation(CalculateCameraDirection(Vector3.forward));
        }
        else
        {
            if (isRunning)
                preMoveSpeed = DefaultMoveSpeed * DefaultFastValue;
            else
                preMoveSpeed = DefaultMoveSpeed;
        }
        this.isAimming = isAimming;
    }
}
