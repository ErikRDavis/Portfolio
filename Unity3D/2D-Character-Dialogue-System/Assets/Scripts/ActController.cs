using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActController : MonoBehaviour
{
    public delegate void OnActCompleted();
    public event OnActCompleted onActCompleted;
    public delegate void OnActOutStep(DialogueStep step);
    public event OnActOutStep onActOutStep;

    public bool ActCompleted { get; private set; }

    private Act currentAct;
    private DialogueStep[] steps;
    private int currentStepIndex = 0;
    private DialogueStep currentStep;

    public void SetAct(Act act)
    {
        currentAct = act;

        steps = currentAct.steps;
    }

    public void StartAct()
    {
        currentStepIndex = 0;
        ActCompleted = false;
        currentStep = steps[currentStepIndex];

        ActOutCurrentStep();
    }

    public void MoveToNextStep()
    {
        if (ActCompleted) return;

        if (currentStepIndex >= steps.Length - 1)
        {// No more steps in the act
            ActCompleted = true;

            onActCompleted?.Invoke();

            return;
        }

        currentStep = steps[++currentStepIndex];

        ActOutCurrentStep();
    }

    private void ActOutCurrentStep()
    {
        onActOutStep?.Invoke(currentStep);
    }
}
