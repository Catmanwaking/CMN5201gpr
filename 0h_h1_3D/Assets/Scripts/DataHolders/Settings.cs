//Author: Dominik Dohmeier

[System.Serializable]
public struct Settings
{
    public int muteSound;
    public int UseStopwatch;
    public int ShowHint;
    public ColorTheme ColorTheme;
    public Language LanguageID;

    public Settings(int mute, int stopwatch, int hint, ColorTheme colorTheme, Language language)
    {
        muteSound = mute;
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