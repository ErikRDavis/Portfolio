using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trebuchet Data", menuName = "Data/Trebuchet Data")]
public class TrebuchetData : ScriptableObject
{
    public bool autoRearm;
    public float minReleaseAngle;
    public float maxReleaseAngle;
    public float defaultReleaseAngle;

    public float maxBodyRotateSpeed;
    [Tooltip("Degrees Per Second")]
    public float rearmSpeed;
    public float updateReleaseAngleSpeed;

    public string munitionKey;
}
