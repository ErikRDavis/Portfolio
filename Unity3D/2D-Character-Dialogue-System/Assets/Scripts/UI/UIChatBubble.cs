using UnityEngine;
using TMPro;

public class UIChatBubble : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] texts;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;

    private Coroutine activeAnimation = null;
    private bool isReversed;

    private void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void UpdateBubble(ActorDialogue dialogue)
    {
        isReversed = dialogue.reverse;

        if (dialogue.texts.Length > texts.Length)
        {
            Debug.LogError($"Chat bubblet text count mismatch! Received text count is greater than bubble's capacity");
        }

        for(int i = 0; i < texts.Length && i < dialogue.texts.Length; i++)
        {
            texts[i].text = dialogue.texts[i];

            SetScale(texts[i].rectTransform);
        }

        SetScale(rectTransform);
        SetPosition(dialogue.bubblePosition);
    }

    private void SetScale(RectTransform rectTransform)
    {
        Vector2 temp = rectTransform.localScale;
        temp.x = isReversed ? -1 : 1;
        rectTransform.localScale = temp;
    }

    private void SetPosition(Vector2 position)
    {
        rectTransform.anchoredPosition = position;
    }

    public void ShowBubble(float time)
    {
        StopAnimation();

        activeAnimation = StartCoroutine(Animations.FadeUIIn(canvasGroup, time, Easings.EaseType.EaseInOutSine));
    }

    private void StopAnimation()
    {
        if(activeAnimation != null)
        {
            StopCoroutine(activeAnimation);

            activeAnimation = null;
        }
    }

    public void HideBubble(float time)
    {
        StopAnimation();

        activeAnimation = StartCoroutine(Animations.FadeUIOut(canvasGroup, time, Easings.EaseType.EaseInOutSine));
    }
}
