//Author: Dominik Dohmeier
using System.Collections;
using TMPro;
using UnityEngine;

public class AutoFontSizeAdjuster : MonoBehaviour
{
    [SerializeField] private float maxFontSize;
    private TMP_Text[] textElements;

    // Start is called before the first frame update
    private void Start()
    {
        textElements = GetComponentsInChildren<TMP_Text>();
        SetChildrenToSmallestFontSize();
        LocalizationSystem.OnLanguageChanged += SetChildrenToSmallestFontSize;
    }

    private void OnDestroy()
    {
        LocalizationSystem.OnLanguageChanged -= SetChildrenToSmallestFontSize;
    }

    private IEnumerator FontSizeRoutine()
    {
        foreach (var text in textElements)
        {
            text.fontSizeMax = maxFontSize;
            text.enableAutoSizing = true;
        }

        yield return null;

        float smallestSize = maxFontSize;
        foreach (var text in textElements)
        {
            if(text.fontSize < smallestSize)
                smallestSize = text.fontSize;
        }

        foreach (var text in textElements)
        {
            text.fontSize = smallestSize;
            text.enableAutoSizing = false;
        }
    }

    private void SetChildrenToSmallestFontSize()
    {
        StartCoroutine(FontSizeRoutine());
    }
}