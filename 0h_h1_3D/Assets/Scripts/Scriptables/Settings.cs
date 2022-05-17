//Author: Dominik Dohmeier

[System.Serializable]
public struct Settings
{ 
    public int UseStopwatch;
    public int ShowHint;
    public ColorTheme ColorTheme;
    public Language LanguageID;

    public Settings(int stopwatch, int hint, ColorTheme colorTheme, Language language)
    {
        UseStopwatch = stopwatch;
        ShowHint = hint;
        ColorTheme = colorTheme;
        LanguageID = language;
    }
}

public enum Language
{
    EN,
    DE
}