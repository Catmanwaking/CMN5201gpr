using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class buttonColorAdjuster : MonoBehaviour
{
    [SerializeField] private int colorIndex;
    private Image image;

    void Start()
    {
        ColorIndex.ColorChanged += OnColorThemeChanged;
        OnColorThemeChanged();
    }

    private void OnDestroy()
    {
        ColorIndex.ColorChanged -= OnColorThemeChanged;
    }

    private void OnColorThemeChanged()
    {
        if (image == null)
            image = GetComponent<Image>();
        Debug.Log("cam color change");
        image.color = ColorIndex.GetColor(colorIndex);
    }
}
