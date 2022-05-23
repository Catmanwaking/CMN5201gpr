using System.IO;
using UnityEngine;

public static class SettingsLoader
{
    private const int DEFAULT_STOPWATCH = 1;
    private const int DEFAULT_HINT = 1;
    private const ColorTheme DEFAULT_COLOR_THEME = ColorTheme.Default;
    private const Language DEFAULT_LANGUAGE = Language.EN;

    private static readonly string path = Application.persistentDataPath;
    private const string fileName = "settings.json";

    private static Settings settings;
    private static bool loaded = false;

    public static Settings LoadSettings()
    {
        if(loaded)
            return settings;

        if (!File.Exists($"{path}/{fileName}"))
        {
            return new Settings
            (
                DEFAULT_STOPWATCH,
                DEFAULT_HINT,
                DEFAULT_COLOR_THEME,
                DEFAULT_LANGUAGE
            );
        }

        string jsonString = File.ReadAllText($"{path}/{fileName}");
        settings = JsonUtility.FromJson<Settings>(jsonString);
        loaded = true;
        return settings;
    }

    public static void SaveSettings(Settings settings)
    {
        SettingsLoader.settings = settings;

        string jsonString = JsonUtility.ToJson(settings, true);
        File.WriteAllText($"{path}/{fileName}", jsonString);        
    }

    public static Language LoadLanguage()
    {
        if (!File.Exists($"{path}/{fileName}"))
            return Language.EN;

        string jsonString = File.ReadAllText($"{path}/{fileName}");
        return JsonUtility.FromJson<Settings>(jsonString).LanguageID;
    }
}