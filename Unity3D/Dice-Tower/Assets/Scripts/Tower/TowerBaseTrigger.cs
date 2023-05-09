using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerBaseTrigger : MonoBehaviour
{
    public UnityEvent<Die> onDieExit;

    private void OnTriggerExit(Collider other)
    {
        if (transform.InverseTransformPoint(other.transform.position).z > 0)
        {
            Die die = other.GetComponent<Die>();

            onDieExit?.Invoke(die);
        }
    }
}
