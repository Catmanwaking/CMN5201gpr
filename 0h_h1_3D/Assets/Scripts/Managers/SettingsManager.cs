//Author: Dominik Dohmeier
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public void CloseSettings()
    {
        SceneManager.UnloadSceneAsync(1, UnloadSceneOptions.None);
    }

    public void ChangeLanguage(string languageID)
    {
        LocalizationSystem.ChangeLanguage(languageID);
    }
}