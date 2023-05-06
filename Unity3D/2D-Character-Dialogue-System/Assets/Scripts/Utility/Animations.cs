using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations
{
    public static IEnumerator FadeUIIn(CanvasGroup canvasGroup, float time, Easings.EaseType easeType = Easings.EaseType.None)
    {
        yield return FadeUIFromTo(canvasGroup, 0, 1, time, easeType);
    }

    public static IEnumerator FadeUIOut(CanvasGroup canvasGroup, float time, Easings.EaseType easeType = Easings.EaseType.None)
    {
        yield return FadeUIFromTo(canvasGroup, 1, 0, time, easeType);
    }

    public static IEnumerator FadeUIFromTo(CanvasGroup canvasGroup, float from, float to, float time, Easings.EaseType easeType = Easings.EaseType.None)
    {
        float currentTime = 0;
        float progress = 0;

        canvasGroup.alpha = from;

        while (currentTime < time)
        {
            progress = Mathf.Clamp01(currentTime / time);

            canvasGroup.alpha = Mathf.Lerp(from, to, Easings.Ease(progress, easeType)); ;

            yield return null;

            currentTime += Time.deltaTime;
        }

        canvasGroup.alpha = to;
    }
}
