//Author: Dominik Dohmeier
using UnityEngine;

public enum ColorTheme
{
    Default,
    Original
}

public static class ColorIndex
{
    private static Color[] colors = new Color[] { Color.gray, Color.green, Color.blue };

    public static int ColorCount { get => colors.Length; }

    public static Color GetColor(int color)
    {
        return colors[color];
    }

    public static void SetColors(ColorTheme theme)
    {
        colors = theme switch
        {
            ColorTheme.Original => new Color[] { Color.gray, Color.red, Color.cyan },
            _ => new Color[] { Color.gray, Color.green, Color.blue },
        };
    }
}