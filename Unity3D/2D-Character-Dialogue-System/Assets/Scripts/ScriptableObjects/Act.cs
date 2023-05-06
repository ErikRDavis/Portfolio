using UnityEngine;

[CreateAssetMenu(fileName = "Act 1", menuName = "ScriptableObjects/Act", order = 1)]
public class Act : ScriptableObject
{
    public Sprite background;

    public DialogueStep[] steps;
}