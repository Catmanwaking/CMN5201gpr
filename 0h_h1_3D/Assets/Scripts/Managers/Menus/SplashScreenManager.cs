using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MenuFader
{
    [Header("CubeDemo")]
    [SerializeField, Tooltip("in degrees per second")] private float rotationSpeed;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private GameObject gimbal;
    [SerializeField] private float endAngle;

    [Header("TextDemo")]
    [SerializeField] private float fadeTime;
    [SerializeField] private TMP_Text left;
    [SerializeField] private TMP_Text right;
    [SerializeField] private TMP_Text center;
    [SerializeField] private RectTransform demoObjectTrans;
    [SerializeField] private GameObject nonDemoObjects;

    void Start()
    {
        FadeIn();        
        StartCoroutine(SplashScreenDemo());
    }

    private IEnumerator SplashScreenDemo()
    {
        yield return new WaitForSeconds(fadeTime);
        yield return FadeRoutine(left);
        yield return FadeRoutine(right);
        StartCoroutine(CubeDemo());
        yield return FadeRoutine(center);

        yield return new WaitForSeconds(fadeTime);
        yield return CenterTextRoutine();
        nonDemoObjects.SetActive(true);
    }

    private IEnumerator FadeRoutine(TMP_Text text)
    {
        Color fadeColor = new(1.0f, 1.0f, 1.0f, 0.0f);
        float startTime = Time.time;
        float endTime = startTime + fadeTime;
        float currentTime = startTime;
        while (currentTime <= endTime)
        {
            fadeColor.a = Mathf.InverseLerp(startTime, endTime, currentTime);
            text.color = fadeColor;
            yield return null;
            currentTime += Time.deltaTime;
        }
        fadeColor.a = 1.0f;
        text.color = fadeColor;
    }

    private IEnumerator CubeDemo()
    {
        float startTime = Time.time;
        float endTime = startTime + fadeTime;
        float currentTime = startTime;
        float angle;
        while (currentTime <= endTime)
        {
            angle = Mathf.InverseLerp(startTime, endTime, currentTime);
            angle = curve.Evaluate(angle) * endAngle;
            gimbal.transform.rotation = Quaternion.Euler(angle, angle, 0.0f);
            yield return null;
            currentTime += Time.deltaTime;
        }
        gimbal.transform.rotation = Quaternion.Euler(endAngle, endAngle, 0.0f);

        while (true)
        {
            gimbal.transform.Rotate(0.0f, rotationSpeed * Time.deltaTime, 0.0f, Space.World);
            yield return null;
        }
    }

    private IEnumerator CenterTextRoutine()
    {
        Vector2 anchorMin = demoObjectTrans.anchorMin;
        Vector2 anchorMax = demoObjectTrans.anchorMax;
        float width = anchorMax.x - anchorMin.x;
        float min = (1.0f - width) * 0.5f;
        float max = 1.0f - min;
        Vector2 newAnchorMin = new(min, anchorMin.y);
        Vector2 newAnchorMax = new(max, anchorMax.y);

        float startTime = Time.time;
        float endTime = startTime + fadeTime;
        float currentTime = startTime;
        while (currentTime <= endTime)
        {
            float t = Mathf.InverseLerp(startTime, endTime, currentTime);
            t = curve.Evaluate(t);
            demoObjectTrans.anchorMin = Vector2.Lerp(anchorMin, newAnchorMin, t);
            demoObjectTrans.anchorMax = Vector2.Lerp(anchorMax, newAnchorMax, t);
            yield return null;
            currentTime += Time.deltaTime;
        }
        demoObjectTrans.anchorMin = newAnchorMin;
        demoObjectTrans.anchorMax = newAnchorMax;
    }

    public void LoadMainMenu()
    {
        OnFadeOutComplete += () => SceneManager.LoadScene((int)SceneIndex.MainMenu);
        FadeOut();
    }
}
