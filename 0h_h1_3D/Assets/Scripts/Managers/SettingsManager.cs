//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private SettingsSO settings;

    public void CloseSettings()
    {
        SceneManager.UnloadSceneAsync(1, UnloadSceneOptions.None);
    }

    public void ChangeLanguage(string languageID)
    {
        settings.LanguageID = languageID;
        LocalizationSystem.ChangeLanguage(languageID);
    }

    public void ChangeStopwatch(string yesNoID)
    {
        settings.UseStopwatch = yesNoID switch
        {
            "ID_Options_Yes" => true,
            "ID_Options_No" => false,
            _ => false,
        };
    }

    public void ChangeHint(string yesNoID)
    {
        settings.ShowHint = yesNoID switch
        {
            "ID_Options_Yes" => true,
            "ID_Options_No" => false,
            _ => false,
        };
    }

    public void ChangeColor(string colorID)
    {
        settings.colorTheme = colorID switch
        {
            "ID_Options_Color_Default" => ColorTheme.Default,
            "ID_Options_Color_0h_h1" => ColorTheme.Original,
            _ => ColorTheme.Default,
        };
    }

    public void LoadSetting()
    {

    }
}