using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ViewTarget : IComparable
{
    public Transform viewTarget;
    public Vector3 targetOffset;
    public Vector3 viewOffset;

    public int CompareTo(object obj)
    {

        if (obj is Transform transform)
        {
            return transform == viewTarget ? 1 : -1;
        }
        else if (obj is ViewTarget vT)
        {
            return vT.viewTarget == viewTarget ? 1 : -1;
        }

        return 0;
    }
}
