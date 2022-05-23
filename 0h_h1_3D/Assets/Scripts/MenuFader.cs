using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class MenuFader : MonoBehaviour
{
    protected event System.Action OnFadeInComplete;
    protected event System.Action OnFadeOutComplete;

    [SerializeField, Range(0.0f, 1.0f)] private float fadeInDuration;
    [SerializeField, Range(0.0f, 1.0f)] private float fadeOutDuration;

    private CanvasGroup canvasGroup;

    protected void FadeIn()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        if (fadeInDuration > 0.0f)
            StartCoroutine(FadeInRoutine());
        else
            canvasGroup.alpha = 1.0f;
    }

    protected void FadeOut()
    {
        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();

        if(fadeOutDuration > 0.0f)
            StartCoroutine(FadeOutRoutine());
        else
            canvasGroup.alpha = 0.0f;
    }

    private IEnumerator FadeInRoutine()
    {
        canvasGroup.alpha = 0.0f;
        float startTime = Time.time;
        float endTime = startTime + fadeInDuration;
        float currentTime = startTime;
        float fade;
        while (currentTime <= endTime)
        {
            fade = Mathf.InverseLerp(startTime, endTime, currentTime);
            canvasGroup.alpha = fade;
            yield return null;
            currentTime += Time.deltaTime;
        }
        canvasGroup.alpha = 1.0f;
        OnFadeInComplete?.Invoke();
    }

    private IEnumerator FadeOutRoutine()
    {
        canvasGroup.alpha = 1.0f;
        float startTime = Time.time;
        float endTime = startTime + fadeOutDuration;
        float currentTime = startTime;
        float fade;
        while (currentTime <= endTime)
        {
            fade = Mathf.InverseLerp(endTime, startTime, currentTime);
            canvasGroup.alpha = fade;
            yield return null;
            currentTime += Time.deltaTime;
        }
        canvasGroup.alpha = 0.0f;
        OnFadeOutComplete?.Invoke();
    }
}