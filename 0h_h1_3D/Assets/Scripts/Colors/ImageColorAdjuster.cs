using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageColorAdjuster : MonoBehaviour
{
    [SerializeField] private ColorType colorIndex;
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
        image.color = ColorIndex.GetColor((int)colorIndex);
    }
}
