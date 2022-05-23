//Author: Dominik Dohmeier
using UnityEngine;

public enum ColorTheme
{
    Default,
    Original
}

public enum ColorType
{
    Empty,
    Primary,
    Secondary,
    Outline,
    Highlight,
    BackGround
}

public static class ColorIndex
{
    public const int ColorCount = 3;
    public static event System.Action ColorChanged;

    private static int currentTheme;
    private static Color[][] colorThemes;

    static ColorIndex()
    {
        colorThemes = new Color[][]
        {
            FromHex("#212416", "#9AAB3A","#9F97BD","#515931","#E6F2BB","#222222"),
            FromHex("#2B2B2B","#d04f2e","#25a2be","#101010","#cfcfcf","#222222"),
        };
    }

    public static Color GetColor(int color)
    {
        return colorThemes[currentTheme][color];
    }

    public static Color GetOutlineColor(int color)
    {
        return colorThemes[currentTheme][ColorCount + color];
    }

    public static void SetColorTheme(ColorTheme theme)
    {
        currentTheme = (int)theme;
        ColorChanged?.Invoke();
    }

    private static Color[] FromHex(params string[] hexCodes)
    {
        Color[] colors = new Color[hexCodes.Length];
        for (int i = 0; i < hexCodes.Length; i++)
            ColorUtility.TryParseHtmlString(hexCodes[i], out colors[i]);
        return colors;
    }
}