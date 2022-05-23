//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MenuFader
{
    [SerializeField] private OptionCycleText stopwatchOCT;
    [SerializeField] private OptionCycleText hintOCT;
    [SerializeField] private OptionCycleText colorThemeOCT;
    [SerializeField] private OptionCycleText languageOCT;

    public void CloseSettings()
    {
        SaveSettings();
        SceneManager.UnloadSceneAsync((int)SceneIndex.Settings, UnloadSceneOptions.None);
    }

    private void Start()
    {
        LoadSettings();
        FadeIn();
    }

    private void ChangeLanguage()
    {
        LocalizationSystem.LoadLanguage(languageOCT.GetSelected());
    }

    private void SaveSettings()
    {
        Settings settings = new()
        {
            UseStopwatch = stopwatchOCT.Index,
            ShowHint = hintOCT.Index,
            ColorTheme = (ColorTheme)colorThemeOCT.Index,
            LanguageID = (Language)languageOCT.Index
        };
        languageOCT.OnOptionChange -= ChangeLanguage;

        SettingsLoader.SaveSettings(settings);
    }

    private void LoadSettings()
    {
        Settings settings = SettingsLoader.LoadSettings();

        stopwatchOCT.Index = settings.UseStopwatch;
        hintOCT.Index = settings.ShowHint;
        colorThemeOCT.Index = (int)settings.ColorTheme;
        languageOCT.Index = (int)settings.LanguageID;

        languageOCT.OnOptionChange += ChangeLanguage;
    }
}