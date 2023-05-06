using UnityEngine;

public class DialogueEvent : ScriptableObject
{
    [Tooltip("Delay in seconds before executing the event")]
    public float delay = 0;
    [Tooltip("When true this event will need to complete before actors are updated")]
    public bool completeBeforeActorUpdate;
}
