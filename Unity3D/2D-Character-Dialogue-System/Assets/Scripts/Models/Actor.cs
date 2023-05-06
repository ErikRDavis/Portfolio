using UnityEngine;

[System.Serializable]
public class Actor
{
    [Tooltip("Name is needed for keeping track of current actors in the scene")]
    public string name;
    public Sprite sprite;
    public Vector2 position;
    public float width = 250;

    public ActorDialogue dialogue;
}