using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBlackoutOverlay : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine currentAnimation = null;

    public void HideOverlay(float time = 0.5f)
    {
        StopAnimation();

        StartCoroutine(FadeOut(time));
    }

    public void ShowOverlay(float time = 0.5f)
    {
        StopAnimation();

        StartCoroutine(FadeIn(time));
    }

    private IEnumerator FadeOut(float time)
    {
        yield return Animations.FadeUIOut(canvasGroup, time, Easings.EaseType.EaseInOutSine);
    }

    private IEnumerator FadeIn(float time)
    {
        yield return Animations.FadeUIIn(canvasGroup, time, Easings.EaseType.EaseInOutSine);
    }

    private void StopAnimation()
    {
        if(currentAnimation != null)
        {
            StopCoroutine(currentAnimation);

            currentAnimation = null;
        }
    }
}
