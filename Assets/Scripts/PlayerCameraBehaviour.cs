using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraBehaviour : MonoBehaviour
{
    [SerializeField] private float RotateSensitive = 5f;

    private float preRotateSensitive = 0;
    private Vector3 rotateDirection = Vector3.zero;
    private Vector3 preCameraRotation = Vector3.zero;

    private void Awake()
    {
        preCameraRotation = transform.rotation.eulerAngles;
        preRotateSensitive = RotateSensitive * Time.deltaTime;
    }

    public void UpdateRotateDirection(Vector2 newRotateDirection)
    {
        rotateDirection = newRotateDirection;
        rotateDirection.z = 0;
    }

    public void FixedUpdate()
    {
        TurnTheCamera();
    }

    private void TurnTheCamera()
    {
        rotateDirection *= preRotateSensitive;
        preCameraRotation.x = Mathf.Clamp(preCameraRotation.x + rotateDirection.x, -90, 90);
        preCameraRotation.y = Mathf.Repeat(preCameraRotation.y + rotateDirection.y, 360);
        transform.rotation = Quaternion.Euler(preCameraRotation);
    }
}
