using UnityEngine;

public class MaterialColorLoader : MonoBehaviour
{
    [SerializeField] private Material[] fontMaterials;
    [SerializeField] private Material[] uiMaterials;

    private static Material[] staticFontMaterialReference;
    private static Material[] staticUIMaterialReference;

    void Start()
    {
        SetReference(fontMaterials, uiMaterials);
        ColorIndex.ColorChanged += OnColorThemeChanged;
        OnColorThemeChanged();
    }

    private static void SetReference(Material[] font, Material[] ui)
    {
        staticFontMaterialReference = font;
        staticUIMaterialReference = ui;
    }

    private static void OnColorThemeChanged()
    {
        Color color = ColorIndex.GetFontColor();
        foreach (Material material in staticFontMaterialReference)
            material.SetColor("_FaceColor", color);
        foreach (Material material in staticUIMaterialReference)
            material.SetColor("_Color", color);
    }
}