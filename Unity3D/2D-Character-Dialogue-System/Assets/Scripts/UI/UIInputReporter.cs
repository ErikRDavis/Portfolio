using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInputReporter : MonoBehaviour, IPointerClickHandler
{
    public delegate void OnInputReceived();
    public event OnInputReceived onInputReceived;

    public void OnPointerClick(PointerEventData eventData)
    {
        onInputReceived?.Invoke();
    }
}
