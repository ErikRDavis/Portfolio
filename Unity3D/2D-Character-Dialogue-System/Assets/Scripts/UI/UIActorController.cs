using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActorController : MonoBehaviour
{
    public delegate void OnActorAdded(UIActor actor);
    public event OnActorAdded onActorAdded;

    [SerializeField]
    private RectTransform actorRoot;
    [SerializeField]
    private UIActor actorPrefab;

    private Dictionary<string, UIActor> actors;
    private List<string> stepActorNames;
    private DialogueStep currentStep;

    private void Awake()
    {
        actors = new Dictionary<string, UIActor>();
        stepActorNames = new List<string>();
    }

    public void UpdateActorsFromStep(DialogueStep step)
    {
        Actor actorData;
        UIActor actor = null;

        currentStep = step;
        stepActorNames.Clear();

        for (int i = 0; i < step.actors.Length; i++)
        {
            actorData = step.actors[i];

            stepActorNames.Add(actorData.name);

            if (actors.ContainsKey(actorData.name) == false)
            {
                actor = AddActorWithData(actorData);
            }
            else
            {
                actor = actors[actorData.name];

                actor.SetData(actorData);
            }

            if(ActorHasDialogue(actor))
            {
                BringActorToForefront(actor);
            }
        }

        RemoveActorsNotPresent();
    }
    
    private UIActor AddActorWithData(Actor actorData)
    {
        UIActor actorObj = Instantiate(actorPrefab, actorRoot);

        actorObj.name = actorData.name;
        actorObj.gameObject.SetActive(true);
        actorObj.SetData(actorData);

        actors.Add(actorData.name, actorObj);

        onActorAdded?.Invoke(actorObj);

        return actorObj;
    }

    public static bool ActorHasDialogue(UIActor actor)
    {
        return actor.Data.dialogue != null && actor.Data.dialogue.texts != null && actor.Data.dialogue.texts.Length > 0;
    }

    private void BringActorToForefront(UIActor actor)
    {
        actor?.transform.SetAsLastSibling();
    }

    private void RemoveActorsNotPresent()
    {
        if (actors.Count == currentStep.actors.Length) return;

        List<string> actorNames = new List<string>(actors.Keys);

        for(int i = 0; i < actorNames.Count; i++)
        {
            if (stepActorNames.Contains(actorNames[i]) == false)
            {
                // The actor is not participating in the act this step
                RemoveActor(actorNames[i]);
            }
        }
    }

    private void RemoveActor(string actorName)
    {
        UIActor actor = actors[actorName];

        actor.RemoveFromSceneInstant();

        actors.Remove(actorName);
    }

    public void ResetForRestart()
    {
        stepActorNames.Clear();

        List<string> actorNames = new List<string>(actors.Keys);

        for (int i = 0; i < actorNames.Count; i++)
        {
            RemoveActor(actorNames[i]);
        }
    }

}
