//Author: Dominik Dohmeier
using UnityEngine;

public static class ColorIndex
{
    private static Color[] colors = new Color[] { Color.gray, Color.red, Color.cyan };

    public static Color GetColor(int color)
    {
        return colors[color];
    }

    public static void SetColors(params Color[] colors)
    {
        ColorIndex.colors = colors;
    }
}

public enum ColorTheme
{
    Default,
    Original
}