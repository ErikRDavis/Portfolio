using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue 1", menuName = "ScriptableObjects/Act Steps/Dialogue/Eventful Dialogue"/*, order = 2*/)]
public class EventfulDialogueStep : DialogueStep
{
    public DialogueEvent[] events;
}
