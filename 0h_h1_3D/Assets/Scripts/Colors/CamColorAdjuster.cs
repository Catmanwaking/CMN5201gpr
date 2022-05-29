using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CamColorAdjuster : MonoBehaviour
{
    private Camera cam;

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
        if (cam == null)
            cam = GetComponent<Camera>();
        cam.backgroundColor = ColorIndex.GetBackGroundColor();
    }
}
