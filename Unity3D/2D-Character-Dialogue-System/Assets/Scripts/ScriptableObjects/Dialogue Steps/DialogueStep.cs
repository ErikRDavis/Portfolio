using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue 1", menuName = "ScriptableObjects/Act Steps/Dialogue/Dialogue"/*, order = 2*/)]
public class DialogueStep : ScriptableObject
{
    public Actor[] actors;
}
