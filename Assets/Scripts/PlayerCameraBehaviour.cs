using System.Collections;
using UnityEngine;

public class PlayerCameraBehaviour : MonoBehaviour
{
    [SerializeField] private float RotateSensitive = 5f;
    [SerializeField] private Transform ZoomTransform = null;
    [SerializeField] private Transform CameraTransform = null;
    [SerializeField] private Transform NormalTransform = null;
    [SerializeField] private Transform CameraBodyTransform = null;

    private bool isAiming = false;
    private float aimingPosX = 0f;
    private float preRotateSensitive = 0f;
    private Vector3 rotateDirection = Vector3.zero;
    private Vector3 preCameraRotation = Vector3.zero;
    private Coroutine aimingZoomCoroutine = null;
    private Coroutine aimingShiftCoroutine = null;

    private void Awake()
    {
        aimingPosX = CameraBodyTransform.localPosition.x;
        preCameraRotation = transform.rotation.eulerAngles;
        preRotateSensitive = RotateSensitive * Time.deltaTime;
    }

    public void FixedUpdate()
    {
        TurnTheCamera();
    }

    public void UpdateRotateDirection(Vector2 newRotateDirection)
    {
        rotateDirection = newRotateDirection;
        rotateDirection.z = 0;
    }

    public void UpdateAimingState(bool isAiming)
    {
        this.isAiming = isAiming;
        if(aimingZoomCoroutine != null)
        {
            StopCoroutine(aimingZoomCoroutine);
        }
        aimingZoomCoroutine = StartCoroutine(AimingZoomCoroutine());
    }

    public void UpdateAimingPosition()
    {
        aimingPosX *= -1;
        if(aimingShiftCoroutine != null)
        {
            StopCoroutine(aimingShiftCoroutine);
        }
        aimingShiftCoroutine = StartCoroutine(AimingShiftCoroutine());
    }

    private void TurnTheCamera()
    {
        rotateDirection *= preRotateSensitive;
        preCameraRotation.x = Mathf.Clamp(preCameraRotation.x + rotateDirection.x, -90, 90);
        preCameraRotation.y = Mathf.Repeat(preCameraRotation.y + rotateDirection.y, 360);
        transform.rotation = Quaternion.Euler(preCameraRotation);
    }

    private IEnumerator AimingShiftCoroutine()
    {
        var preCameraPos = CameraBodyTransform.localPosition;

        if(aimingPosX > 0)
        {
            while(preCameraPos.x < aimingPosX)
            {
                preCameraPos.x = Mathf.Lerp(preCameraPos.x, aimingPosX, .1f);
                CameraBodyTransform.localPosition = preCameraPos;
                yield return null;
            }
        }
        else
        {
            while(preCameraPos.x > aimingPosX)
            {
                preCameraPos.x = Mathf.Lerp(preCameraPos.x, aimingPosX, .1f);
                CameraBodyTransform.localPosition = preCameraPos;
                yield return null;
            }
        }

        preCameraPos.x = aimingPosX;
        CameraBodyTransform.localPosition = preCameraPos;
        yield return null;
    }

    private IEnumerator AimingZoomCoroutine()
    {
        Transform targetTransform;
        if (isAiming)
        {
            targetTransform = ZoomTransform;
        }
        else
        {
            targetTransform = NormalTransform;
        }

        var distMagnitude = Vector3.SqrMagnitude(targetTransform.localPosition - CameraTransform.localPosition);
        while (distMagnitude > 1f)
        {
            CameraTransform.localPosition = Vector3.Slerp(CameraTransform.localPosition, targetTransform.localPosition, .07f);
            distMagnitude = Vector3.SqrMagnitude(targetTransform.localPosition - CameraTransform.localPosition);
            yield return null;
        }

        CameraTransform.localPosition = targetTransform.localPosition;
        yield return null;
    }
}
