//Author: Dominik Dohmeier

[System.Serializable]
public struct Settings
{ 
    public int UseStopwatch;
    public int ShowHint;
    public int ColorTheme;
    public Language LanguageID;

    public Settings(int stopwatch, int hint, int colorTheme, Language language)
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