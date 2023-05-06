using System.Collections;
using UnityEngine;

public class UIScreenFlasher : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private float maxBrightness;
    private float minBrightness;
    private float frequency;
    private Coroutine flashRoutine = null;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup.alpha = 0;
    }

    public void FlashScreen(float frequency, float minBrightness = 0, float maxBrightness = 1)
    {
        StopFlash();

        this.frequency = frequency;
        this.minBrightness = minBrightness;
        this.maxBrightness = maxBrightness;

        flashRoutine = StartCoroutine(AnimateScreenFlash());
    }

    public void StopFlash()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
    }

    public void ClearFlash()
    {
        StopFlash();

        canvasGroup.alpha = 0;
    }

    private IEnumerator AnimateScreenFlash()
    {
        yield return Flicker(frequency, minBrightness, maxBrightness);
    }

    private IEnumerator Flicker(float frequency, float min, float max)
    {
        float time = 1 / frequency;

        yield return Animations.FadeUIFromTo(canvasGroup, 0, min, time);

        while (true)
        {
            yield return Animations.FadeUIFromTo(canvasGroup, canvasGroup.alpha, Random.Range(min, max), time);
        }
    }
}
