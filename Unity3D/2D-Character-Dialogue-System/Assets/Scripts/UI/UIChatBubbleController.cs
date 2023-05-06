using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChatBubbleController : MonoBehaviour
{
    [SerializeField]
    private RectTransform bubbleRoot;

    private List<UIChatBubble> bubbles;

    private void Start()
    {
        bubbles = new List<UIChatBubble>();
    }

    public void ActorAdded(UIActor actor)
    {
        actor.onTransitionEnd += Actor_onTransitionEnd;
    }

    private void Actor_onTransitionEnd(UIActor actor)
    {
        // if the actor has dialogue create the bubble, position it, set its text, and make it visible
        if(UIActorController.ActorHasDialogue(actor))
        {
            AddChatBubble(actor);
        }
    }

    private void AddChatBubble(UIActor actor)
    {
        ActorDialogue dialogue = actor.Data.dialogue;

        if (!dialogue.bubblePrefab)
        {
            Debug.LogError($"AddChatBubble() - Missing bubble prefab for \"{actor.Data.name}\"");
            return;
        }

        UIChatBubble bubble = Instantiate(dialogue.bubblePrefab, bubbleRoot);

        bubble.UpdateBubble(dialogue);

        bubble.ShowBubble(0.2f);

        bubbles.Add(bubble);
    }

    public void ClearCurrentBubbles()
    {
        for(int i = bubbles.Count - 1; i >= 0; i--)
        {
            Destroy(bubbles[i].gameObject);
        }

        bubbles.Clear();
    }
}
