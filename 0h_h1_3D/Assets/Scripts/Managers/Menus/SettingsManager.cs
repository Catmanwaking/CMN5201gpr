//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MenuFader
{
    [SerializeField] private Image backGround;
    [SerializeField] private OptionCycleText muteOCT;
    [SerializeField] private OptionCycleText stopwatchOCT;
    [SerializeField] private OptionCycleText hintOCT;
    [SerializeField] private OptionCycleText colorThemeOCT;
    [SerializeField] private OptionCycleText languageOCT;

    public void CloseSettings()
    {
        SaveSettings();
        OnFadeOutComplete += () => SceneManager.UnloadSceneAsync((int)SceneIndex.Settings, UnloadSceneOptions.None);
        FadeOut();
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

    private void ChangeMute()
    {
        AudioManager.SetMute(muteOCT.Index == 1);
    }

    private void ChangeColor()
    {
        ColorIndex.SetColorTheme((ColorTheme)colorThemeOCT.Index);
        backGround.color = ColorIndex.GetBackGroundColor();
    }

    private void SaveSettings()
    {
        Settings settings = new()
        {
            muteSound = muteOCT.Index,
            UseStopwatch = stopwatchOCT.Index,
            ShowHint = hintOCT.Index,
            ColorTheme = (ColorTheme)colorThemeOCT.Index,
            LanguageID = (Language)languageOCT.Index
        };
        languageOCT.OnOptionChange -= ChangeLanguage;
        muteOCT.OnOptionChange -= ChangeMute;
        colorThemeOCT.OnOptionChange -= ChangeColor;

        SettingsLoader.SaveSettings(settings);
    }

    private void LoadSettings()
    {
        Settings settings = SettingsLoader.LoadSettings();

        backGround.color = ColorIndex.GetBackGroundColor();

        muteOCT.Index = settings.muteSound;
        stopwatchOCT.Index = settings.UseStopwatch;
        hintOCT.Index = settings.ShowHint;
        colorThemeOCT.Index = (int)settings.ColorTheme;
        languageOCT.Index = (int)settings.LanguageID;

        languageOCT.OnOptionChange += ChangeLanguage;
        muteOCT.OnOptionChange += ChangeMute;
        colorThemeOCT.OnOptionChange += ChangeColor;
    }
}