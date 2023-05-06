using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIActor : MonoBehaviour
{
    public delegate void OnTransitionEnd(UIActor actor);
    public event OnTransitionEnd onTransitionEnd;

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Image image;
    [SerializeField]
    private CanvasGroup canvasGroup;

    public Actor Data { get; private set; }

    public Vector2 AnchoredPosition => rectTransform.anchoredPosition;

    private Actor previousData = null;

    public void SetData(Actor actorData)
    {
        previousData = Data;

        Data = actorData;

        UpdateFromData();
    }

    private void UpdateFromData()
    {
        if (previousData == null)
        {
            image.overrideSprite = Data.sprite;

            SetPosition(Data.position);
            SetActorSize();

            StartCoroutine(RevealNewActor(0.1f));
        }
        else if (ActorHasChanged())
        {
            StartCoroutine(UpdateVisual(0.15f));
        }
    }

    public void SetPosition(Vector2 position)
    {
        rectTransform.anchoredPosition = position;
    }

    private void SetActorSize()
    {
        Vector2 temp = rectTransform.sizeDelta;
        temp.x = Data.width;
        rectTransform.sizeDelta = temp;

        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }

    private IEnumerator RevealNewActor(float time)
    {
        yield return FadeActorIn(time);

        onTransitionEnd?.Invoke(this);
    }

    private IEnumerator FadeActorIn(float time)
    {        
        yield return Animations.FadeUIIn(canvasGroup, time, Easings.EaseType.EaseInOutSine);
    }

    private bool ActorHasChanged()
    {
        bool differentSprite = image.overrideSprite != Data.sprite;
        bool differentPosition = previousData != null ? previousData.position != Data.position : true;
        bool differentSize = previousData != null ? previousData.width != Data.width : true;

        return differentSprite || differentPosition || differentSize;
    }

    private IEnumerator UpdateVisual(float time, Easings.EaseType easeType = Easings.EaseType.None)
    {
        float halfTime = time / 2;

        yield return FadeActorOut(halfTime);

        image.overrideSprite = Data.sprite;

        SetPosition(Data.position);
        SetActorSize();

        yield return FadeActorIn(halfTime);

        onTransitionEnd?.Invoke(this);
    }

    private IEnumerator FadeActorOut(float time)
    {
        yield return Animations.FadeUIOut(canvasGroup, time, Easings.EaseType.EaseInOutSine);
    }

    public void RemoveFromSceneInstant()
    {
        Destroy(gameObject);
    }

    public void RemoveFromScene()
    {
        StartCoroutine(LeaveScene());
    }

    protected virtual IEnumerator LeaveScene()
    {
        yield return FadeActorOut(0.2f);

        Destroy(gameObject);
    }
}
