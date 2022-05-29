//Author: Dominik Dohmeier
using UnityEngine;

public enum ColorTheme
{
    Default,
    Original,
    LightMode,
    Deuteranomaly,
    Tritanomaly,
}

public enum ColorType
{
    Empty,
    Primary,
    Secondary,
    Outline,
    Highlight,
    BackGround,
    Font
}

public static class ColorIndex
{
    public static event System.Action ColorChanged;
    public const int ColorCount = 3;

    private readonly static Color[][] colorThemes;
    private static ColorTheme currentTheme;

    static ColorIndex()
    {
        colorThemes = new Color[][]
        {
            FromHex("#212416", "#9AAB3A", "#9F97BD", "#515931", "#E6F2BB", "#222222", "#FFFFFF"),
            FromHex("#2B2B2B", "#D04F2E", "#25A2BE", "#101010", "#CFCFCF", "#222222", "#FFFFFF"),
            FromHex("#2B2B2B", "#D04F2E", "#25A2BE", "#101010", "#CFCFCF", "#FFFFFF", "#222222"),
            FromHex("#2B2B2B", "#B67032", "#3E81BA", "#101010", "#CFCFCF", "#222222", "#FFFFFF"),
            FromHex("#2B2B2B", "#CB4634", "#29A9B8", "#101010", "#CFCFCF", "#222222", "#FFFFFF"),
        };
        currentTheme = SettingsLoader.LoadSettings().ColorTheme;
    }

    public static Color GetColor(int color)
    {
        return colorThemes[(int)currentTheme][color];
    }

    public static Color GetOutlineColor(int color)
    {
        return colorThemes[(int)currentTheme][(int)ColorType.Outline + color];
    }

    public static Color GetBackGroundColor()
    {
        return colorThemes[(int)currentTheme][(int)ColorType.BackGround];
    }

    public static Color GetFontColor()
    {
        return colorThemes[(int)currentTheme][(int)ColorType.Font];
    }

    public static void SetColorTheme(ColorTheme theme)
    {
        currentTheme = theme;
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