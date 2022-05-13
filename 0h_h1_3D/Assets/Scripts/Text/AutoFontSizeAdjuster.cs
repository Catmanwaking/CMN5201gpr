//Author: Dominik Dohmeier
using TMPro;
using UnityEngine;

public class AutoFontSizeAdjuster : MonoBehaviour
{
    [SerializeField] private float fontSize;
    private TMP_Text[] textElements;

    // Start is called before the first frame update
    private void Start()
    {
        SetChildrenToSmallestFontSize();
    }

    private void SetChildrenToSmallestFontSize()
    {
        textElements = GetComponentsInChildren<TMP_Text>();

        fontSize = float.MaxValue;
        foreach (var text in textElements)
        {
            if (text.fontSize < fontSize)
                fontSize = text.fontSize;
        }

        foreach (var text in textElements)
            text.fontSizeMax = fontSize;
    }
}