//Author: Dominik Dohmeier
using UnityEngine;

public enum ColorTheme
{
    Default,
    Original
}

public static class ColorIndex
{
    public static event System.Action ColorChanged;

    private static int currentTheme;

    private static Color[][] colorThemes;

    public const int ColorCount = 3;

    static ColorIndex()
    {
        colorThemes = new Color[][]
        {
            new Color[] { Color.gray, Color.green, Color.blue },
            new Color[] { new Color32(41, 41, 41,0), new Color32(208, 79, 46, 0), new Color32(37, 162, 190, 0) },
        };
    }

    public static Color GetColor(int color)
    {
        return colorThemes[currentTheme][color];
    }

    public static void SetColorTheme(ColorTheme theme)
    {
        currentTheme = (int)theme;
        ColorChanged?.Invoke();
    }
}