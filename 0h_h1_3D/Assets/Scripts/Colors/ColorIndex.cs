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
    ButtonColor,
    Font
}

public static class ColorIndex
{
    public static event System.Action ColorChanged;
    public const int ColorCount = 3;

    private static Color[] currentColorTheme;

    static ColorIndex()
    {
        ColorTheme currentTheme = SettingsLoader.LoadSettings().ColorTheme;
        SetColorTheme(currentTheme);
    }

    public static Color GetColor(int color)
    {
        return currentColorTheme[color];
    }

    public static Color GetOutlineColor(int color)
    {
        return currentColorTheme[(int)ColorType.Outline + color];
    }

    public static Color GetBackGroundColor()
    {
        return currentColorTheme[(int)ColorType.BackGround];
    }

    public static Color GetFontColor()
    {
        return currentColorTheme[(int)ColorType.Font];
    }

    public static void SetColorTheme(ColorTheme theme)
    {
        currentColorTheme = theme switch
        {
            ColorTheme.Default =>       FromHex("#303030", "#E61919", "#E6DF19", "#000000", "#E6F2BB", "#222222", "#1C1C1C", "#FFFFFF"),
            ColorTheme.Original =>      FromHex("#2B2B2B", "#D04F2E", "#25A2BE", "#101010", "#CFCFCF", "#222222", "#1C1C1C", "#FFFFFF"),
            ColorTheme.LightMode =>     FromHex("#2B2B2B", "#D04F2E", "#25A2BE", "#101010", "#CFCFCF", "#FFFFFF", "#BCBCBC", "#222222"),
            ColorTheme.Deuteranomaly => FromHex("#2B2B2B", "#B67032", "#3E81BA", "#101010", "#CFCFCF", "#222222", "#1C1C1C", "#FFFFFF"),
            ColorTheme.Tritanomaly =>   FromHex("#2B2B2B", "#CB4634", "#29A9B8", "#101010", "#CFCFCF", "#222222", "#1C1C1C", "#FFFFFF"),
            _ =>                        FromHex("#303030", "#E61919", "#E6DF19", "#000000", "#E6F2BB", "#222222", "#1C1C1C", "#FFFFFF")
        };
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