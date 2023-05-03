using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trebuchet))]
public class EditorTrebuchet : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Trebuchet trebuchet = (Trebuchet)target;

        if(!Application.isPlaying)
        {
            if (GUILayout.Button("Set Arm Armed Rotation"))
            {
                trebuchet.Editor_SetArmArmedRotation();
            }

            if (GUILayout.Button("Set Sling Armed Rotation"))
            {
                trebuchet.Editor_SetSlingArmedRotation();
            }
        }

        
    }
}
