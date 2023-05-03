using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Rig Data", menuName = "Data/Camera Rig Data")]
public class CameraRigData : ScriptableObject
{
    public float maxRotateSpeed;
    public float positionTrackingSpeed;
}
