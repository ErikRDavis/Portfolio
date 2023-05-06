using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public delegate void OnInputReceived();
    public event OnInputReceived onInputReceived;

    [SerializeField]
    private Image background;
    [SerializeField]
    private UIBlackoutOverlay blackoutOverlay;
    [SerializeField]
    private UICinematicBars cinematicBars;
    [SerializeField]
    private UIScreenFlasher screenFlasher;
    [SerializeField]
    private UIInputReporter inputReporter;
    [SerializeField]
    private UIActorController actorController;
    [SerializeField]
    private UIChatBubbleController chatBubbleController;

    private Dictionary<Type, Action> afterEventActions;
    private bool actorsHaveBeenUpdated = false;

    private void Awake()
    {
        afterEventActions = new Dictionary<Type, Action>();

        inputReporter.onInputReceived += InputReporter_onInputReceived;
        actorController.onActorAdded += ActorController_onActorAdded;
        cinematicBars.onAnimationComplete += OnDialogueEventComplete;
    }

    private void InputReporter_onInputReceived()
    {
        onInputReceived?.Invoke();
    }

    private void ActorController_onActorAdded(UIActor actor)
    {
        chatBubbleController.ActorAdded(actor);
    }

    private void OnDialogueEventComplete(Type type)
    {
        if(afterEventActions.ContainsKey(type))
        {
            Action action = afterEventActions[type];

            action?.Invoke();

            afterEventActions.Remove(type);
        }
    }

    private void Start()
    {
        cinematicBars.HideBars(0);
    }

    public void SetBackground(Sprite background)
    {
        this.background.overrideSprite = background;
    }

    public void ShowBlackoutOverlay()
    {
        blackoutOverlay.ShowOverlay();
    }

    public void ResetForRestart()
    {
        cinematicBars.HideBars(0);
        chatBubbleController.ClearCurrentBubbles();
        afterEventActions.Clear();
    }

    public void ActOutStep(DialogueStep step)
    {
        ResetForNewStep();

        // If the step does not have events or if none of the events have to finish before actors are updated then update the actors
        if ((step is EventfulDialogueStep) == false || PerformEvents((EventfulDialogueStep)step) == false)
        {
            UpdateActorsFromStep(step);
        }      
    }

    private void ResetForNewStep()
    {
        screenFlasher.ClearFlash();
        chatBubbleController.ClearCurrentBubbles();
        actorsHaveBeenUpdated = false;
    }

    private void UpdateActorsFromStep(DialogueStep step)
    {
        if (actorsHaveBeenUpdated) return;

        actorsHaveBeenUpdated = true;

        actorController.UpdateActorsFromStep(step);
    }

    /// <summary>
    /// Performs all of the events in the eventful dialogue step
    /// </summary>
    /// <param name="dialogueStep"></param>
    /// <returns>Bool value for if at least one event was required to complete before updating the actors</returns>
    protected bool PerformEvents(EventfulDialogueStep dialogueStep)
    {
        int waitToUpdateActorsCount = 0;

        for (int i = 0; i < dialogueStep.events.Length; i++)
        {
            waitToUpdateActorsCount += dialogueStep.events[i].completeBeforeActorUpdate ? 1 : 0;

            RegisterAfterEventAction(dialogueStep.events[i], dialogueStep);
            
            StartCoroutine(PerformEvent(dialogueStep.events[i]));
        }

        return waitToUpdateActorsCount > 0;
    }

    protected virtual void RegisterAfterEventAction(DialogueEvent dialogueEvent, EventfulDialogueStep eventfulDialogue)
    {
        Type eventType = dialogueEvent.GetType();

        if(afterEventActions.ContainsKey(eventType))
        {
            Debug.LogError($"UIController - RegisterAfterEventAction() - An after-event-action already exists for \"{eventType}\"");
            return;
        }

        if (dialogueEvent.completeBeforeActorUpdate)
        {
            afterEventActions.Add(eventType, () =>
            {
                UpdateActorsFromStep(eventfulDialogue);
            });
        }
    }

    protected virtual IEnumerator PerformEvent(DialogueEvent dialogueEvent) 
    {
        if(dialogueEvent.delay > 0)
        {
            yield return new WaitForSeconds(dialogueEvent.delay);
        }

        if(dialogueEvent is ShowCinematicBarsEvent)
        {
            cinematicBars.ShowBars();
        }
        else if(dialogueEvent is FlashScreenEvent)
        {
            screenFlasher.FlashScreen(5, 0.4f, 0.70f);
        }
        else if(dialogueEvent is FadeOutOverlayEvent)
        {
            blackoutOverlay.HideOverlay();
        }
        else if(dialogueEvent is FadeInOverlayEvent)
        {
            blackoutOverlay.ShowOverlay();
        }
    }

}
