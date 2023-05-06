using System;
using System.Collections;
using UnityEngine;

public class UICinematicBars : MonoBehaviour
{
    public delegate void OnAnimationComplete(Type eType);
    public event OnAnimationComplete onAnimationComplete;

    [SerializeField]
    private RectTransform topBar;
    [SerializeField]
    private RectTransform bottomBar;

    private Vector2 topStart = new Vector2();
    private Vector2 bottomStart = new Vector2();
    private Vector2 topEnd = new Vector2();
    private Vector2 bottomEnd = new Vector2();
    float topHiddenPosition;
    float bottomHiddenPosition;

    private void Start()
    {
        topHiddenPosition = topBar.sizeDelta.y + 5;
        bottomHiddenPosition = -bottomBar.sizeDelta.y - 5;
    }

    public void ShowBars(float time = 0.5f)
    {
        StartCoroutine(AnimateBarsIn(time));
    }

    public void HideBars(float time = 0.5f)
    {
        StartCoroutine(AnimateBarsOut(time));
    }

    private IEnumerator AnimateBarsIn(float time)
    {
        topStart.y = topHiddenPosition;
        bottomStart.y = bottomHiddenPosition;
        topEnd.y = 0;
        bottomEnd.y = 0;

        topBar.gameObject.SetActive(true);
        bottomBar.gameObject.SetActive(true);

        yield return AnimateBars(time);

        onAnimationComplete?.Invoke(typeof(ShowCinematicBarsEvent));
    }

    private IEnumerator AnimateBarsOut(float time)
    {
        topStart.y = 0;
        bottomStart.y = 0;
        topEnd.y = topHiddenPosition;
        bottomEnd.y = bottomHiddenPosition;

        yield return AnimateBars(time);

        topBar.gameObject.SetActive(false);
        bottomBar.gameObject.SetActive(false);

        onAnimationComplete?.Invoke(typeof(HideCinematicBarsEvent));
    }

    private IEnumerator AnimateBars(float time)
    {
        float currentTime = 0;
        float progress = 0;

        topBar.anchoredPosition = topStart;
        bottomBar.anchoredPosition = bottomStart;

        while (currentTime < time)
        {
            progress = Mathf.Clamp01(currentTime / time);
            topBar.anchoredPosition = Vector2.Lerp(topStart, topEnd, Easings.EaseInOutSine(progress));
            bottomBar.anchoredPosition = Vector2.Lerp(bottomStart, bottomEnd, Easings.EaseInOutSine(progress));

            yield return null;

            currentTime += Time.deltaTime;
        }

        topBar.anchoredPosition = topEnd;
        bottomBar.anchoredPosition = bottomEnd;
    }
}
