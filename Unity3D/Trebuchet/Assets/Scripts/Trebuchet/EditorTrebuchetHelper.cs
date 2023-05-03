using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Trebuchet
{
#if UNITY_EDITOR
    public void Editor_SetArmArmedRotation()
    {
        Debug.Log($"Trebuchet - set arm armed rotation");
        armArmedAngle = arm.localRotation;
    }

    public void Editor_SetSlingArmedRotation()
    {
        Debug.Log($"Trebuchet - set sling armed rotation");
        slingArmedAngle = sling.localRotation;
    }

#endif
}
