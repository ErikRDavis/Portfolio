using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private ActController actController;

    [SerializeField]
    private Act act;

    [SerializeField]
    private float stepCooldown = 1.0f;

    private bool canMoveToNextStep = true;
    private bool actCompleted = false;

    private void Awake()
    {
        uiController.onInputReceived += UiController_onInputReceived;

        actController.onActOutStep += ActController_onActOutStep;
        actController.onActCompleted += ActController_onActCompleted;
    }

    private void Start()
    {
        PlayDialogue(act);
    }

    public void PlayDialogue(Act dialogueAct)
    {
        uiController.SetBackground(dialogueAct.background);

        actController.SetAct(dialogueAct);
        actController.StartAct();
    }

    private void UiController_onInputReceived()
    {
        if (!canMoveToNextStep) return;

        StartCoroutine(RunCantMoveTimer());

        if(actCompleted == false)
        {
            actController.MoveToNextStep();
        }
        else
        {
            RestartAct();
        }
    }

    private IEnumerator RunCantMoveTimer()
    {
        canMoveToNextStep = false;

        yield return new WaitForSeconds(stepCooldown);

        canMoveToNextStep = true;
    }

    private void RestartAct()
    {
        uiController.ResetForRestart();
        actCompleted = false;

        PlayDialogue(act);
    }

    protected virtual void ActController_onActCompleted()
    {
        Debug.Log("Act Completed!");

        actCompleted = true;

        uiController.ShowBlackoutOverlay();

        StartCoroutine(RunCantMoveTimer());
    }

    private void ActController_onActOutStep(DialogueStep step)
    {
        uiController.ActOutStep(step);
    }

}
