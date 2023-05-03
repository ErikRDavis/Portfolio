using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraRig : MonoBehaviour
{
    [SerializeField]
    private Camera rigCamera;
    [SerializeField]
    private Transform orbitRoot;
    [SerializeField]
    private Transform cameraTarget;

    private CameraRigData data;
    private Vector3 cameraOffset;
    private Vector3 targetOffset;
    private Vector2 rotationInput = new Vector2();

    public void SetData(CameraRigData data)
    {
        this.data = data;
    }

    public void SetTarget(Transform target, Vector3 targetOffset, Vector3 camOffset, Vector2? pose = null)
    {
        cameraTarget = target;
        this.targetOffset = targetOffset;
        cameraOffset = camOffset;

        transform.position = target.position;

        if(pose != null)
        {
            Vector2 newPose = (Vector2)pose;
            SetRigAngles(newPose.x, newPose.y);
        }
    }

    private void SetRigAngles(float x, float y)
    {
        transform.Rotate(Vector3.up, x);
        orbitRoot.Rotate(Vector3.right, y);
    }

    public void SetCameraRotationInput(Vector2 rotationInput)
    {
        this.rotationInput = rotationInput;
    }

    private void LateUpdate()
    {
        if (data == null) return;

        if (rotationInput.sqrMagnitude > 0)
        {
            UpdateCameraRigPose();
        }
    }

    private void UpdateCameraRigPose()
    {
        SetRigAngles(data.maxRotateSpeed * rotationInput.x * Time.deltaTime, data.maxRotateSpeed * -rotationInput.y * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        UpdateCameraRigPosition();
    }

    private void UpdateCameraRigPosition()
    {
        if (!cameraTarget) return;

        transform.position = Vector3.Lerp(transform.position, cameraTarget.position, data.positionTrackingSpeed * Time.deltaTime);
        rigCamera.transform.localPosition = Vector3.Lerp(rigCamera.transform.localPosition, cameraOffset, data.positionTrackingSpeed * Time.deltaTime);

        LookAtTarget();
    }

    private void LookAtTarget()
    {
        rigCamera.transform.LookAt(cameraTarget.position + targetOffset);
    }

}
