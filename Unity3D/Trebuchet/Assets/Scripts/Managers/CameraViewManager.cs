using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewManager : MonoBehaviour
{
    [SerializeField]
    private CameraRig cameraRig;
    [SerializeField]
    private CameraRigData data;

    public List<ViewTarget> viewTargets;

    private int currentViewIndex = 0;
    private ViewTarget currentViewTarget = null;

    private void Start()
    {
        cameraRig.SetData(data);

        SwitchCameraView(0, new Vector2(20, 30));
    }

    public void SwitchCameraView(int input, Vector2? pose = null)
    {
        try
        {
            input = Mathf.Clamp(input, -1, 1);

            if (currentViewIndex == 0 && input < 0)
            {
                currentViewIndex = viewTargets.Count - 1;
            }
            else if (currentViewIndex >= viewTargets.Count - 1 && input > 0)
            {
                currentViewIndex = 0;
            }
            else
            {
                currentViewIndex += input;
            }

            SetCurrentTarget(viewTargets[currentViewIndex], pose);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error switching to Camera View: {currentViewIndex} - Viewable targets: {viewTargets.Count} - {ex.Message + ex.StackTrace}");
        }
    }

    private void SetCurrentTarget(ViewTarget viewTarget, Vector2? pose = null)
    {
        currentViewTarget = viewTarget;

        cameraRig.SetTarget(viewTarget.viewTarget, viewTarget.targetOffset, viewTarget.viewOffset, pose);
    }

    public void RotateCamera(Vector2 value)
    {
        cameraRig.SetCameraRotationInput(value);
    }

    public void AddCameraViewTarget(Transform target, Vector3 targetOffset, Vector3 camOffset)
    {
        ViewTarget vT = new ViewTarget();
        vT.viewTarget = target;
        vT.targetOffset = targetOffset;
        vT.viewOffset = camOffset;

        viewTargets.Add(vT);
    }

    public void RemoveCameraViewTarget(Transform target)
    {
        bool found = false;
        for(int i = 0; !found && i < viewTargets.Count; i++)
        {
            if (viewTargets[i].CompareTo(target) == 1)
            {
                found = true;
                RemoveViewTarget(i);
            }
        }
    }

    private void RemoveViewTarget(int index)
    {
        ViewTarget target = viewTargets[index];
        viewTargets.RemoveAt(index);

        if (currentViewTarget.CompareTo(target) == 1)
        {
            currentViewIndex = 0;

            SetCurrentTarget(viewTargets[currentViewIndex]);
        }
    }
}
